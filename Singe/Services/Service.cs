using Singe.Services.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe.Services
{
    public static class Service
    {
        internal static List<Command> RegisteredCommands = new List<Command>();
        internal static InvocationBuilder TreeBuilder = new InvocationBuilder();
        internal static object lastResult;
        internal static Dictionary<Key, string> keyCommandBindings = new Dictionary<Key, string>();

        static Service()
        {
            RegisterAssembly(Assembly.GetEntryAssembly());
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

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
                        if (attribute.AttributeType == typeof(CommandAttribute))
                        {
                            var attr = method.GetCustomAttribute<CommandAttribute>();

                            if (RegisteredCommands.Any(c => c.Signature.Name.ToLower() == method.Name.ToLower() && c.Signature.ServiceName.ToLower() == attr.serviceName.ToLower()))
                            {
                                Console.WriteLine($"Command {method.Name.ToLower()} is already present in the service '{attr.serviceName.ToLower()}'");
                            }
                            else 
                            {
                                RegisterMethod(method);
                            } 
                        }
                    }
                } 
            }
        }

        private static void RegisterMethod(MethodInfo info)
        {
            Command command;
            
            var attr = info.GetCustomAttribute<CommandAttribute>();
            
            if (attr.serviceName != "")
            {
                command = new Command(info, attr.serviceName);
            }
            else
            {
                command = new Command(info, info.DeclaringType.Name);
            }

            RegisteredCommands.Add(command);
        }

        public static void SubmitCommandString(string commandString)
        {
            try
            {
                var invocations = TreeBuilder.GetInvocations(commandString);

                foreach (var invocation in invocations)
                {
                    if (!InvokeCommand(invocation))
                    {
                        //Console.WriteLine("Unrecognized command");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static bool InvokeCommand(CommandInvocation invocation)
        {
            Command command;
            if (invocation.Service == "")
            {
                var candidates = RegisteredCommands.Where(c => c.Signature.Name == invocation.Name.ToLower());
                
                if(candidates.Count() < 1)
                {
                    Console.WriteLine("Unrecognized Command");
                    return false;
                }

                if(candidates.Count() > 1)
                {
                    Console.WriteLine("Ambiguous reference to two or more commands.");
                    return false;
                }

                command = candidates.First();
            }
            else
            {
                command = RegisteredCommands.Find(c => c.Signature.ServiceName == invocation.Service.ToLower() && c.Signature.Name == invocation.Name.ToLower());
            }

            if (command != null)
            {
                lastResult = command.Invoke(lastResult, invocation.Parameters.ToArray());
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void BindCommandToKey(Key key, string command)
        {
            if(command == null)
            {
                keyCommandBindings.Remove(key);
            }

            if (keyCommandBindings.ContainsKey(key))
            {
                keyCommandBindings[key] = command;
            }

            keyCommandBindings.Add(key, command);
        }

        internal static void CallKeyCommandBindings()
        {
            foreach (var entry in keyCommandBindings)
            {
                if(Input.GetKeyDown(entry.Key))
                {
                    SubmitCommandString(entry.Value);
                }
            }
        }
    }
}