using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe.Messaging
{

    internal static class AssemblySearch
    {
        public static event EventHandler<AssemblySearchEventArgs> AssemblyAdded;

        static List<Assembly> registeredAssemblies = new List<Assembly>();
        static List<Type> registeredTypes = new List<Type>();
        static AssemblySearch()
        {
            AddAssembly(Assembly.GetEntryAssembly());
            AddAssembly(typeof(AssemblySearch).Assembly);
        }

        public static void AddAssembly(Assembly assembly)
        {
            registeredAssemblies.Add(assembly);

            foreach (var t in assembly.DefinedTypes)
            {
                registeredTypes.Add(t);
            }

            try 
            {
                Commander.Service.RegisterAssembly(assembly);
            }
            catch
            {

            }
        }

        public static Type[] GetTypesWithAttribute<T>() where T : Attribute
        {
            List<Type> result = new List<Type>();

            foreach (var assembly in registeredAssemblies)
            {
                result.AddRange(assembly.GetTypes().Where(t => t.GetCustomAttribute<T>() != null));
            }

            return result.ToArray();
        }

        public static IReadOnlyList<Assembly> GetAssemblies()
        {
            return registeredAssemblies.ToArray();
        }

        public static Type[] GetTypesWithBase(Type baseType)
        {
            List<Type> result = new List<Type>();

            foreach (var t in registeredTypes)
            {
                if(baseType.IsAssignableFrom(t))
                {
                    result.Add(t);
                }
            }

            return result.ToArray();
        }
    }
}
