using System;

namespace SuperIOC.SuperContainer
{
    public class Dependency
    {
        public Dependency(Type abstractType, Type implType)
        {
            AbstractType = abstractType;
            ImplType = implType;
        }

        public Type AbstractType { get; set; }
        public Type ImplType { get; set; }
        public LifeTime LifeTime { get; set; } = LifeTime.Transient;
        private object? Instance { get; set; } = null;

        public object GetOrCreate() => GetOrCreate(new object[] { });

        public object GetOrCreate(object[] ctorParams)
        {
            if (LifeTime is LifeTime.Transient)
            {
                return Creator(ctorParams);
            }

            Instance ??= Creator(ctorParams);
            return Instance;
        }

        private object Creator(object[] ctorParameters)
        {
            object? obj = Activator.CreateInstance(ImplType, ctorParameters);
            if (obj is null) throw ThrowHelper.ActivationError(this);
            return obj;
        }
    }
}