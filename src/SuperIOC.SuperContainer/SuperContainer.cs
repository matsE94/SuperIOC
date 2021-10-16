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
            _registrations[dependency.AbstractType] = dependency;
        }

        public IDependencyProvider BuildProvider()
        {
            return new DependencyProvider(_registrations);
        }
    }
}