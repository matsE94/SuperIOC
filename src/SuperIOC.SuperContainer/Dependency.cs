using System;

namespace SuperIOC.SuperContainer
{
    public class Dependency
    {
        public Dependency(Type abstractType, Type implType)
        {
            AbstractType = abstractType;
            ImplType = implType;
            Creator = BuildDefaultCreator();
        }

        public Type AbstractType { get; set; }
        public Type ImplType { get; set; }
        public LifeTime LifeTime { get; set; } = LifeTime.Transient;
        public object? Instance { get; set; } = null;

        public Func<object[], object> Creator { get; init; }

        private Func<object[], object> BuildDefaultCreator()
        {
            return ctorParameters =>
            {
                object? obj = Activator.CreateInstance(ImplType, ctorParameters);
                if (obj is null) throw ThrowHelper.ActivationError(this);
                return obj;
            };
        }
    }
}