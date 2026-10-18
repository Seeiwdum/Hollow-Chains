using System;
using System.Collections.Generic;
using UnityEngine;

namespace HollowChains.Core.Services
{
    /// <summary>
    /// Small registry for the few true globals, registered once from the _Boot scene.
    /// Used instead of static Instance singletons so services are swappable (tests, mocks) and explicit.
    /// Register by interface type: <c>ServiceLocator.Register&lt;ISaveService&gt;(save)</c>.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> Services = new Dictionary<Type, IService>();

        public static void Register<T>(T service) where T : class, IService
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            var key = typeof(T);
            if (Services.ContainsKey(key))
                throw new InvalidOperationException($"Service {key.Name} is already registered.");
            Services[key] = service;
        }

        public static void Unregister<T>() where T : class, IService => Services.Remove(typeof(T));

        public static bool TryGet<T>(out T service) where T : class, IService
        {
            if (Services.TryGetValue(typeof(T), out var found))
            {
                service = (T)found;
                return true;
            }
            service = null;
            return false;
        }

        public static T Get<T>() where T : class, IService
        {
            if (TryGet<T>(out var service)) return service;
            throw new InvalidOperationException($"Service {typeof(T).Name} is not registered. Is the _Boot scene loaded?");
        }

        /// <summary>Removes every service. Called on play-mode start so Enter Play Mode options without domain reload stay clean.</summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear() => Services.Clear();
    }
}
