using System;

namespace ObjectSync
{
    public class Sync
    {
        public void Execute(object source, object destination)
        {
            if (source == null || destination == null)
                return;

            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (sourceProperty.Name != destinationProperty.Name)
                    {
                        continue;
                    }
                    var val = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, val);
                }
            }
        }
    }
}
