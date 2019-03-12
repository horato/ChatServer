using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace Chat.Core.DependencyInjection
{
    public static class DI
    {
        private static IUnityContainer _instance;

        public static IUnityContainer Container
        {
            get
            {
                if (_instance == null)
                    _instance = new UnityContainer();

                return _instance;
            }
        }

        public static void InstallFrom(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                InstallFrom(assembly);
            }
        }

        public static void InstallFrom(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => !x.IsAbstract && typeof(IDependencyInstaller).IsAssignableFrom(x));
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as IDependencyInstaller;
                if (instance == null)
                    throw new InvalidOperationException($"Failed to create IDependencyInstaller instance from {type}");

                instance.Install(Container);
            }
        }
    }
}
