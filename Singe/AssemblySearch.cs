using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe
{
    internal static class AssemblySearch
    {
        private static List<Assembly> registeredAssemblies = new List<Assembly>();

        public static bool IsAssemblyRegistered(Assembly assembly)
        {
            return registeredAssemblies.Contains(assembly);
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            registeredAssemblies = new List<Assembly>();
        }
    }
}
