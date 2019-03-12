using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsInheritedFromGenericBaseClass(this Type type, Type baseClassType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == baseClassType)
                return true;
            if (type.BaseType != null)
                return IsInheritedFromGenericBaseClass(type.BaseType, baseClassType);

            return false;
        }

        public static Type GetGenericBaseClassArgument(this Type type, Type baseClassType, int position)
        {
            var arguments = type.GetGenericArguments();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == baseClassType && arguments.Length != 0 && position < arguments.Length)
                return arguments[position];

            if (type.BaseType != null)
                return GetGenericBaseClassArgument(type.BaseType, baseClassType, position);

            return null;
        }
    }
}
