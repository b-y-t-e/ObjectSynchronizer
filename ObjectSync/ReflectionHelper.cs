using System;
using Fasterflect;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectSync
{
    public class SyncOmitAttribute : Attribute
    {

    }
    public class SyncKeyAttribute : Attribute
    {

    }

    public static class ReflectionHelper
    {
        public static (TT Attribute, PropertyInfo Property) GetAttributeForProperty<TT>(this object obj)
        {
            if (obj == null)
                return new(default(TT), null);

            PropertyInfo prop = null;
            Type type = null;

            prop = obj as PropertyInfo;

            if (prop == null)
                type = obj is Type ?
                    (Type)obj :
                    obj.GetType();

            if (prop == null)
                prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .FirstOrDefault(p => p.GetCustomAttributes(typeof(TT), false).Count() == 1);

            object ret = prop?.GetCustomAttributes(typeof(TT), false).FirstOrDefault(); //  prop != null ? prop.GetValue(obj, null) : null;
            if(ret == null)
                return new(default(TT), null);

            return new((TT)ret, prop);
        }
        public static bool Equals2(this Object obj1, Object obj2)
        {
            if (obj1 == null && obj2 == null)
                return true;

            else if (obj1 != null && obj2 == null)
                return false;

            else if (obj1 == null && obj2 != null)
                return false;

            return obj1.Equals(obj2);
        }
        public static bool IsList(this Type type)
        {
            if (type == typeof(string))
                return false;

            return type.Implements<IList>();
        }

        public static bool IsCollection(this Type type)
        {
            if (type == typeof(string))
                return false;

            return type.Implements(typeof(ICollection<>));
        }


        public static bool Is<T>(this object obj)
        {
            if (obj == null)
                return false;
            return obj.GetType().Is(typeof(T));
        }

        public static bool Is<T>(this Type thisType)
        {
            return Is(thisType, typeof(T));
        }

        public static bool Is(this object obj, Type baseType)
        {
            if (obj == null)
                return false;
            return obj.GetType().Is(baseType);
        }

        public static bool Is(this Type thisType, Type baseType)
        {
            bool result =
                baseType == thisType ||
                baseType.IsAssignableFrom(thisType);

            if (!result)
            {
                if (thisType.IsInterface && thisType.IsGenericType)
                {
                    Type genericType = thisType.GetGenericTypeDefinition();

                    if (genericType != null)
                        result =
                            baseType == genericType ||
                            baseType.IsAssignableFrom(genericType);
                }
                else
                {
                    foreach (Type interfaceType in thisType.GetInterfaces())
                        if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == baseType)
                            return true;
                        else if (baseType == interfaceType || baseType.IsAssignableFrom(interfaceType))
                            return true;
                }

                /*foreach( var iterface in thisType.GetInterfaces())
                {
                    result =
                        baseType == iterface ||
                        baseType.IsAssignableFrom(iterface);

                    if (result)
                        return true;
                }*/
            }

            return result;
        }
        public static bool IsClass(this Type type)
        {
            if (type == typeof(string))
                return false;

            return type.IsClass;
        }

        public static bool Implements<TInterface>(this Type type)
        {
            if (!typeof(TInterface).IsGenericType)
                return typeof(TInterface).IsAssignableFrom(type);
            return type.GetInterfaces().Any(i =>
            {
                return typeof(TInterface).IsAssignableFrom(i.GetGenericTypeDefinition());
            });
        }

        public static bool Implements(this Type type, Type interfaceType)
        {
            if (!interfaceType.IsGenericType)
                return interfaceType.IsAssignableFrom(type);

            return type.GetInterfaces().Any(i =>
            {
                if (!i.IsGenericType)
                    return false;
                var iGeneric = i.GetGenericTypeDefinition();
                var r = interfaceType == iGeneric || interfaceType.IsAssignableFrom(iGeneric);
                return r;
            });
        }
    }
}
