using System;

namespace SuperIOC.SuperContainer
{
    public static class SuperContainerExtensions
    {
        public static void RegisterTransient<T>
            (this ISuperContainer superContainer, Func<IDependencyProvider,object> creator)
            where T : class
            => superContainer.RegisterTransient<T, T>(creator);

        public static void RegisterTransient<TAbstraction, TImplementation>
            (this ISuperContainer superContainer, Func<IDependencyProvider, object> creator)
            where TImplementation : TAbstraction =>
            superContainer.Register(new Dependency(typeof(TAbstraction), typeof(TImplementation),creator)
            {
                LifeTime = LifeTime.Transient
            });

        public static void RegisterTransient<T>(this ISuperContainer superContainer)
            where T : class
            => superContainer.RegisterTransient<T, T>();

        public static void RegisterTransient<TAbstraction, TImplementation>(this ISuperContainer superContainer)
            where TImplementation : TAbstraction =>
            superContainer.Register(new Dependency(typeof(TAbstraction), typeof(TImplementation))
            {
                LifeTime = LifeTime.Transient
            });

        public static void RegisterSingleton<T>(this ISuperContainer superContainer)
            where T : class
            => superContainer.RegisterSingleton<T, T>();

        public static void RegisterSingleton<TAbstraction, TImplementation>(this ISuperContainer superContainer)
            where TImplementation : TAbstraction =>
            superContainer.Register(new Dependency(typeof(TAbstraction), typeof(TImplementation))
            {
                LifeTime = LifeTime.Singleton
            });
    }
}