using System;
using System.Linq;

namespace SuperIOC.SuperContainer
{
    public static class TypeExtensions
    {
        public static bool HasEmptyConstructor(this Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length > 1)
            {
                throw ThrowHelper.MultipleCtorsError(type);
            }
            var ctor = constructors.FirstOrDefault(x => x.GetParameters().Length == 0);
            return ctor is not null;
        }
    
    }
}