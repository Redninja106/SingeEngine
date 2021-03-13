using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Singe.Services
{
    public static class Service
    {
        public static Dictionary<string, MethodInfo> MethodLookup = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Searches an assembly for commands and registers those commands. By default, the assembly returned by <see cref="Assembly.GetEntryAssembly"/> is searched. Libraries should call this upon their initialization.
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    foreach (var attribute in method.CustomAttributes)
                    {
                        if(attribute.AttributeType == typeof(CommandAttribute))
                        {
                            RegisterMethod(method);
                        }
                    }
                } 
            }
        }

        public static void RegisterMethod(MethodInfo info)
        {
            MethodLookup.Add(SignatureToString(info), info);
        }

        private static string SignatureToString(MethodInfo methodInfo)
        {
            string result = methodInfo.ReturnType.AssemblyQualifiedName;
            result += " " + methodInfo.Name.ToLower();

            foreach (var p in methodInfo.GetParameters())
            {
                result += " " + p.ParameterType.AssemblyQualifiedName;
            }

            return result;
        }

        private static string SignatureToString(Type returnType, string name, Type[] parameters)
        {
            string result = returnType.AssemblyQualifiedName;
            result += " " + name.ToLower();

            foreach (var p in parameters)
            {
                result += " " + p.AssemblyQualifiedName;
            }

            return result;
        }

        private static (Type returnType, string name, Type[] parameters) StringToSignature(string signature)
        {
            var parts = signature.Split(' ');

            Type returnType = Type.GetType(parts[1]);
            string name = parts[2];
            Type[] parameters = new Type[parts.Length - 2];

            for (int i = 2; i < parameters.Length; i++)
            {
                parameters[i] = Type.GetType(parts[i]);
            }

            return (returnType, name, parameters);
        }

        public static void CallCommand(string commandString)
        {
            
        }
    }
}
