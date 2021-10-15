namespace SuperIOC.SuperContainer
{
    public static class SuperContainerExtensions
    {
        public static void RegisterTransient<TAbstraction, TImplementation>(this ISuperContainer superContainer)
            where TImplementation : TAbstraction =>
            superContainer.Register(new Dependency(typeof(TAbstraction),typeof(TImplementation))
            {
                LifeTime = LifeTime.Transient
            });

        public static void RegisterSingleton<TAbstraction, TImplementation>(this ISuperContainer superContainer)
            where TImplementation : TAbstraction =>
            superContainer.Register(new Dependency(typeof(TAbstraction),typeof(TImplementation))
            {
                LifeTime = LifeTime.Singleton
            });

        public static T GetOrThrow<T>(this ISuperContainer superContainer)
            where T : class
        {
            var instance = superContainer.Get<T>();
            if(instance is null) throw ThrowHelper.ActivationError(typeof(T));
            return instance;
        }



    }
}