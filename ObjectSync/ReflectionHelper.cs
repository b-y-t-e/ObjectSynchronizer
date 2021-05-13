using System;
using System.Collections;
using System.Linq;

namespace ObjectSync
{
    public static class ReflectionHelper
    {
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
