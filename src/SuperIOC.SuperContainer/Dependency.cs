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

        public Dependency(Type abstractType, Type implType, Func<IDependencyProvider, object> customCreator)
        {
            AbstractType = abstractType;
            ImplType = implType;
            CustomCreator = customCreator;
        }

        public Type AbstractType { get; set; }
        public Type ImplType { get; set; }
        public Func<IDependencyProvider, object>? CustomCreator { get; set; } = null;
        public LifeTime LifeTime { get; set; } = LifeTime.Transient;
        public object? Instance { get; set; } = null;

        public object GetOrCreate(IDependencyProvider provider) => GetOrCreate(new object[] { },provider);

        public object GetOrCreate(object[] ctorParams, IDependencyProvider provider)
        {
            if (LifeTime is LifeTime.Transient)
            {
                return Creator(ctorParams,provider);
            }

            Instance ??= Creator(ctorParams,provider);
            return Instance;
        }

        private object Creator(object[] ctorParameters, IDependencyProvider provider)
        {
            if (CustomCreator is not null)
            {
                return CustomCreator(provider);
            }
            
            object? obj = Activator.CreateInstance(ImplType, ctorParameters);
            if (obj is null) throw ThrowHelper.ActivationError(this);
            return obj;
        }
    }
}