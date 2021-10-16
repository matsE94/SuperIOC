using System;
using System.Collections.Generic;

namespace SuperIOC.SuperContainer
{
    public interface ISuperContainer
    {
        void Register(Dependency dependency);
        IDependencyProvider BuildProvider();
    }

    public class SuperContainer : ISuperContainer
    {
        private readonly Dictionary<Type, Dependency> _registrations = new();

        public void Register(Dependency dependency)
        {
            var type = dependency.AbstractType;

            if (_registrations.ContainsKey(type))
            {
                throw ThrowHelper.AlreadyRegisteredError(type);
            }

            _registrations[type] = dependency;
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(_registrations);
        }
    }
}