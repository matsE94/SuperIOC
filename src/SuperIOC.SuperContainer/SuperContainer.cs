using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace SuperIOC.SuperContainer
{
    public interface ISuperContainer
    {
        void Register(Dependency dependency);
        IDependencyProvider BuildProvider();
    }

    public interface IDependencyProvider
    {
        T Get<T>() where T : class;
    }

    public class DependencyProvider : IDependencyProvider
    {
        private readonly Dictionary<Type,Dependency> _registrations;

        public DependencyProvider(Dictionary<Type, Dependency> registrations)
        {
            _registrations = registrations;
        }


        public T Get<T>() where T : class
        {
            var abstractType = typeof(T);
            EnsureRegistered(abstractType);

            var dependency = _registrations[abstractType];

            if (dependency.ImplType.HasEmptyConstructor()) return (T)dependency.GetOrCreate(this);
            return (T)ResolveObject(abstractType);
        }

        internal object ResolveObject(Type abstractType)
        {
            var rootRegistration = _registrations[abstractType];

            var ctor = rootRegistration.ImplType.GetConstructors().SingleOrDefault() ??
                       throw ThrowHelper.MultipleCtorsError(abstractType);

            var parameters = ctor.GetParameters();
            var activatedParams = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].ParameterType;
                EnsureRegistered(type);

                var registration = _registrations[type];
                activatedParams[i] = registration.ImplType.HasEmptyConstructor()
                    ? registration.GetOrCreate(this)
                    : ResolveObject(type);
            }

            return rootRegistration.GetOrCreate(activatedParams, this);
        }

        internal void EnsureRegistered(Type abstractType)
        {
            var isRegistered = _registrations.ContainsKey(abstractType);
            if (!isRegistered) throw ThrowHelper.NotRegisteredError(abstractType);
        }
    }

    public class SuperContainer : ISuperContainer
    {
        private readonly Dictionary<Type, Dependency> _registrations = new();

        public void Register(Dependency dependency)
        {
            _registrations[dependency.AbstractType] = dependency;
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(_registrations);
        }
    }
}