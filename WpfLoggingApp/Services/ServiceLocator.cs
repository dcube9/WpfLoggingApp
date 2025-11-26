using System;
using System.Collections.Generic;

namespace WpfLoggingApp.Services
{
    public class ServiceLocator
    {
        private static readonly Lazy<ServiceLocator> instance = new Lazy<ServiceLocator>(() => new ServiceLocator());
        private readonly Dictionary<Type, object> services;
        private readonly Dictionary<Type, Type> serviceTypes;
        private readonly object lockObject;

        private ServiceLocator()
        {
            services = new Dictionary<Type, object>();
            serviceTypes = new Dictionary<Type, Type>();
            lockObject = new object();
        }

        public static ServiceLocator Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void RegisterSingleton<TInterface, TImplementation>() 
            where TImplementation : TInterface, new()
        {
            var serviceType = typeof(TInterface);
            var implementationType = typeof(TImplementation);
            
            lock (lockObject)
            {
                if (!services.ContainsKey(serviceType))
                {
                    serviceTypes[serviceType] = implementationType;
                }
            }
        }

        public T Resolve<T>()
        {
            var serviceType = typeof(T);
            
            lock (lockObject)
            {
                if (services.ContainsKey(serviceType))
                {
                    return (T)services[serviceType];
                }
                
                if (serviceTypes.ContainsKey(serviceType))
                {
                    var implementationType = serviceTypes[serviceType];
                    var instance = Activator.CreateInstance(implementationType);
                    services[serviceType] = instance;
                    return (T)instance;
                }
            }
            
            throw new InvalidOperationException($"Service of type {serviceType.Name} is not registered.");
        }
    }
}
