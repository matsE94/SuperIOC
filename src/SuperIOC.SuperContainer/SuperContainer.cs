﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperIOC.SuperContainer
{
    public interface ISuperContainer
    {
        void Register(Dependency dependency);
        T Get<T>() where T : class;
    }

    public class SuperContainer : ISuperContainer
    {
        private readonly Dictionary<Type, Dependency> _registrations = new();

        public void Register(Dependency dependency)
        {
            _registrations[dependency.AbstractType] = dependency;
        }

        public T Get<T>() where T : class
        {
            var abstractType = typeof(T);
            var isRegistered = _registrations.ContainsKey(abstractType);
            if (!isRegistered)
                throw ThrowHelper.NotRegisteredError(abstractType);
            
            var dependency = _registrations[abstractType];

            if (!HasEmptyCtor(dependency.ImplType)) return (T)ResolveObject(abstractType, false);

            return (T)ResolveObjectNoCtor(dependency);
        }

        internal object ResolveObject(Type abstractType, bool? hasEmptyCtor = null)
        {
            var dependency = _registrations[abstractType];

            if (hasEmptyCtor != null && (bool)hasEmptyCtor || HasEmptyCtor(dependency.ImplType))
            {
                return ResolveObjectNoCtor(dependency);
            }

            var ctor = dependency.ImplType.GetConstructors().SingleOrDefault() ??
                       throw ThrowHelper.MultipleCtorsError(dependency);
            var ctorParams = ctor.GetParameters();
            var activatedParams = new object[ctorParams.Length];
            var i = 0;
            foreach (var param in ctorParams)
            {
                var paramType = param.ParameterType;
                var isRegistered = _registrations.ContainsKey(paramType);
                if (!isRegistered)
                    throw ThrowHelper.NotRegisteredError(paramType);
                activatedParams[i++] = ResolveObject(paramType);
            }

            return dependency.Creator(activatedParams);
        }

        private object ResolveObjectNoCtor(Dependency dependency)
        {
            if (dependency.LifeTime is LifeTime.Transient)
            {
                return dependency.Creator(new object[] { });
            }

            dependency.Instance ??= dependency.Creator(new object[] { });
            return dependency.Instance;
        }

        private bool HasEmptyCtor(Type type)
        {
            var emptyCtor = type.GetConstructors().SingleOrDefault(x => x.GetParameters().Length == 0);
            return emptyCtor is not null;
        }
    }
}