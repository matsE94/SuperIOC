using System;

namespace SuperIOC.SuperContainer
{
    internal static class ThrowHelper
    {
        internal static SuperContainerException ActivationError(Dependency dependency)
        {
            var msg = $"Failed to activate {dependency.ImplType} as {dependency.AbstractType}";
            return new SuperContainerException(msg);
        }

        internal static SuperContainerException ActivationError(Type type)
        {
            var msg = $"Failed to activate {type}";
            return new SuperContainerException(msg);
        }

        internal static SuperContainerException MultipleCtorsError(Type type)
        {
            var msg = $"Multiple constructors are NOT supported. Type {type} has more than one ctors.";
            return new SuperContainerException(msg);
        }

        internal static Exception NotRegisteredError(Type type)
        {
            var msg = $"Cannot activate because ctor param {type} is not registered";
            return new SuperContainerException(msg);
        }
    }
}