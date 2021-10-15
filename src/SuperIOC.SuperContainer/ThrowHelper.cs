using System;

namespace SuperIOC.SuperContainer
{
    public static class ThrowHelper
    {
        public static SuperContainerException ActivationError(Dependency dependency)
        {
            var msg = $"Failed to activate {dependency.ImplType} as {dependency.AbstractType}";
            return new SuperContainerException(msg);
        }

        public static SuperContainerException ActivationError(Type type)
        {
            var msg = $"Failed to activate {type}";
            return new SuperContainerException(msg);
        }

        public static SuperContainerException MultipleCtorsError(Dependency dependency)
        {
            var msg = $"Multiple constructors are NOT supported. Type {dependency.ImplType} has more than one ctors.";
            return new SuperContainerException(msg);
        }

        public static Exception NotRegisteredError(Type type)
        {
            var msg = $"Cannot activate because ctor param {type} is not registered";
            return new SuperContainerException(msg);
        }
    }
}