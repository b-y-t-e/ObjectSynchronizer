using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectSync
{
    public class Sync
    {
        public void Execute(object source, object destination)
        {
            Execute(source, destination, new SyncOptions());
        }

        public void Execute(object source, object destination, SyncOptions syncOptions)
        {
            if (source == null || destination == null)
                return;

            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            var sourceProperties = sourceType.GetProperties();
            var destinationProperties = destinationType.GetProperties();

            var sourcePropertiesDict = sourceProperties.
                ToDictionary(p => p.Name, p => p);

            var destinationPropertiesDict = destinationProperties.
                ToDictionary(p => p.Name, p => p);

            foreach (var propertyName in sourcePropertiesDict.Keys)
            {
                PropertyInfo sourceProperty = null;
                PropertyInfo destinationProperty = null;

                sourcePropertiesDict.TryGetValue(propertyName, out sourceProperty);
                destinationPropertiesDict.TryGetValue(propertyName, out destinationProperty);

                if (sourceProperty == null || destinationProperty == null)
                    continue;

                if (sourceProperty.PropertyType.IsList())
                {
                    CopyListToDestination(source, destination, syncOptions, sourceProperty, destinationProperty);
                }
                else if (sourceProperty.PropertyType.IsClass())
                {
                    CopyObjectToDestination(source, destination, syncOptions, sourceProperty, destinationProperty);
                }
                else
                {
                    CopyPropertyToDestination(source, destination, sourceProperty, destinationProperty, syncOptions);
                }
            }
        }

        private void CopyListToDestination(object source, object destination, SyncOptions syncOptions, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var sourceList = sourceProperty.GetValue(source) as IList;
            var destinationList = destinationProperty.GetValue(destination) as IList;

            if (sourceList == null)
            {
                destinationProperty.SetValue(destination, null);
            }
            else
            {
                if (destinationList == null)
                {
                    destinationList = Activator.CreateInstance(destinationProperty.PropertyType) as IList; ;
                    destinationProperty.SetValue(destination, destinationList);
                }

                var itemsToAdd = BuildItemsToAdd(syncOptions, sourceList, destinationList);
                var itemsToDelete = BuildItemsToDelete(syncOptions, sourceList, destinationList);
                var itemsToUpdate = BuildItemsToUpdate(syncOptions, sourceList, destinationList);

                foreach (var item in itemsToAdd)
                    destinationList.Add(item);

                foreach (var item in itemsToDelete)
                    destinationList.Remove(item);

                foreach (var item in itemsToUpdate)
                    Execute(item.source, item.destination, syncOptions);
            }
        }

        private List<object> BuildItemsToAdd(SyncOptions syncOptions, IList sourceList, IList destinationList)
        {
            var itemsToAdd = new List<object>();
            foreach (var sourceItem in sourceList)
            {
                bool foundSourceItem = false;
                foreach (var destinationItem in destinationList)
                {
                    bool areItemsSame = false;
                    if (syncOptions.TypeKeyDelegate == null)
                    {
                        areItemsSame = sourceItem.Equals2(destinationItem);
                    }
                    else
                    {
                        var sourceKey = sourceItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(sourceItem.GetType()))?.
                            GetValue(sourceItem);

                        var destinationKey = destinationItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(destinationItem.GetType()))?.
                            GetValue(destinationItem);

                        areItemsSame = sourceKey.Equals2(destinationKey);
                    }

                    if (areItemsSame)
                    {
                        foundSourceItem = true;
                        break;
                    }
                }

                if (!foundSourceItem)
                    itemsToAdd.Add(sourceItem);
            }

            return itemsToAdd;
        }

        private List<(object source, object destination)> BuildItemsToUpdate(SyncOptions syncOptions, IList sourceList, IList destinationList)
        {
            var itemsToUpdate = new List<(object source, object destination)>();
            foreach (var sourceItem in sourceList)
            {
                foreach (var destinationItem in destinationList)
                {
                    bool areItemsSame = false;
                    if (syncOptions.TypeKeyDelegate == null)
                    {
                        areItemsSame = sourceItem.Equals2(destinationItem);
                    }
                    else
                    {
                        var sourceKey = sourceItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(sourceItem.GetType()))?.
                            GetValue(sourceItem);

                        var destinationKey = destinationItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(destinationItem.GetType()))?.
                            GetValue(destinationItem);

                        areItemsSame = sourceKey.Equals2(destinationKey);
                    }

                    if (areItemsSame)
                    {
                        itemsToUpdate.Add((sourceItem, destinationItem));
                        break;
                    }
                }
            }

            return itemsToUpdate;
        }


        private List<object> BuildItemsToDelete(SyncOptions syncOptions, IList sourceList, IList destinationList)
        {
            var itemsToDelete = new List<object>();

            foreach (var destinationItem in destinationList)
            {
                bool foundDestinationItem = false;
                foreach (var sourceItem in sourceList)
                {
                    bool areItemsSame = false;
                    if (syncOptions.TypeKeyDelegate == null)
                    {
                        areItemsSame = sourceItem.Equals2(destinationItem);
                    }
                    else
                    {
                        var sourceKey = sourceItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(sourceItem.GetType()))?.
                            GetValue(sourceItem);

                        var destinationKey = destinationItem.GetType().
                            GetProperty(syncOptions.TypeKeyDelegate(destinationItem.GetType()))?.
                            GetValue(destinationItem);

                        areItemsSame = sourceKey.Equals2(destinationKey);
                    }

                    if (areItemsSame)
                    {
                        foundDestinationItem = true;
                        break;
                    }
                }

                if (!foundDestinationItem)
                    itemsToDelete.Add(destinationItem);
            }

            return itemsToDelete;
        }

        private void CopyObjectToDestination(object source, object destination, SyncOptions syncOptions, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var sourceValue = sourceProperty.GetValue(source);
            var destinationValue = destinationProperty.GetValue(destination);

            if (sourceValue == null)
            {
                destinationProperty.SetValue(destination, null);
            }
            else
            {
                if (destinationValue == null)
                {
                    destinationValue = Activator.CreateInstance(destinationProperty.PropertyType);
                    destinationProperty.SetValue(destination, destinationValue);
                }

                Execute(sourceValue, destinationValue, syncOptions);
            }
        }

        private static void CopyPropertyToDestination(object source, object destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty, SyncOptions syncOptions)
        {
            if (syncOptions?.TypeFieldFilterDelegate != null &&
                (syncOptions.TypeFieldFilterDelegate(destination.GetType(), destinationProperty.Name) == false ||
                 syncOptions.TypeFieldFilterDelegate(destination.GetType(), sourceProperty.Name) == false))
                return;

            var sourceValue = sourceProperty.GetValue(source);
            destinationProperty.SetValue(destination, sourceValue);
        }
    }

    public class SyncOptions
    {
        public bool MaintainListOrder { get; set; }

        public Func<Type, string> TypeKeyDelegate { get; set; }

        public Func<Type, String, bool> TypeFieldFilterDelegate { get; set; }
    }
}
