using System;
using System.Collections.Generic;

namespace PixelSnapper.WPF.Utilities
{
    public static class ServiceLocator
    {
        private readonly static Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public static T Resolve<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
