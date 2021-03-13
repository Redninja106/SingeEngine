using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe.Services.old
{
    public class Service
    {
        public static object GetLastCommandResult()
        {
            return lastResult;
        }

        private static object lastResult;

        static List<Service> services = new List<Service>();

        static Service()
        {
            // register every service in this namespace.
            foreach (var t in typeof(Service).Assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Service)))
                {
                    t.GetConstructor(Array.Empty<Type>())?.Invoke(null);
                }
            }
        }

        public Service()
        {
            services.Add(this);
        }

        public static bool SubmitCommand(string commandString)
        {
            if (commandString == null || commandString == "")
                return false;

            var commands = commandString.Split('|');
            bool success = true;

            foreach (var command in commands)
            {
                if (success == false)
                    return false;

                var input = command.Split("\"");
                List<string> s = new List<string>();

                for (int i = 0; i < input.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        s.AddRange(input[i].Trim().Split(" "));
                    }
                    else
                    {
                        s.Add(input[i].Trim());
                    }
                }

                s.RemoveAll(s => s.Length == 0 || char.IsWhiteSpace(s[0]));

                if (s.Count == 0)
                    success = false;
                else if (s.Count <= 1)
                    success = CallCommmand(s[0], Array.Empty<string>());
                else
                    success = CallCommmand(s[0], s.ToArray()[1..]);
            }

            return true;
        }

        private static string GetServiceName(string name)
        {
            if(name.EndsWith("service") && name.LastIndexOf("service") != 0)
            {
                name = name.Remove(name.LastIndexOf("service"));
            }

            return name;
        }

        public static bool CallCommmand(string command, string[] args)
        {
            try
            {
                string serviceName = "";
                if (command.Contains(":"))
                {
                    var parts = command.Split(":");
                    serviceName = parts[0].Trim();
                    command = parts[1].Trim();

                    var service = services.Where(s => GetServiceName(s.GetType().Name.ToLower()) == serviceName.ToLower());

                    if (service.Count() == 1)
                    {
                        service.First().CallLocalCommand(command, args);
                    }
                    else
                    {
                        throw new Exception("No commands of name \"" + command + "\" on the service \"" + serviceName + "\".");
                    }
                }
                else
                {
                    // try to find the service that contains the command
                    List<Service> candidates = new List<Service>();
                    MethodInfo info;
                    foreach (var s in services)
                    {
                        if(s.TryGetCommand(command, args, out _))
                        {
                            candidates.Add(s);
                        }
                    }

                    if (candidates.Count == 0)
                    {
                        throw new Exception("No services found with the command: \"" + command + "\"");
                    }

                    if(candidates.Count > 1)
                    {
                        // format exception string
                        string exString = "Ambiguous reference to the following commands:\n";

                        foreach (var c in candidates)
                        {
                            exString += "   " + c.GetType().Name + ":" + command + "\n";
                        }

                        throw new Exception(exString);
                    }

                    candidates.First().CallLocalCommand(command, args);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Executes a command with the given name and arguments on this service.
        /// </summary>
        public void CallLocalCommand(string commandName, string[] args)
        {
            // get all methods on this object
            var commands = this.GetType().GetMethods().AsEnumerable();

            // filter out non command methods like ToString and Equals
            commands = commands.Where(m => m.DeclaringType != typeof(Service) && m.DeclaringType != typeof(object));

            // find the all of the commands with the right name
            commands = commands.Where(m => m.Name.ToLower() == commandName.ToLower());

            // finally, filter by arg count
            commands = commands.Where(m => m.GetParameters().Count() == args.Count());

            // if there is no method that matches this at this point, the command doesn't exist
            if(commands.Count() < 1)
            {
                throw new Exception("Unrecognized command");
            }

            // if there is more than 1 method that meets these parameters, throw
            if (commands.Count() > 1) 
            {
                // format exception string
                string exString = "Ambiguous reference to the following commands:\n";

                foreach (var c in commands)
                {
                    exString += "   " + this.GetType().Name + ":" +  c.Name;

                    foreach (var p in c.GetParameters())
                    {
                        exString += " " + p.ParameterType.Name;
                    }

                    exString += '\n';
                }

                throw new Exception(exString);
            }

            var command = commands.First();

            // prep arguments
            object[] parameters = new object[args.Length];

            var ProcessParamsResult = ProcessParameters(command, parameters, args);

            // failure if innerException is not equal to null
            if(ProcessParamsResult.innerException != null)
            {
                throw new Exception("Unable to parse argument on command \"" + commandName + "\" at index " + ProcessParamsResult.index, ProcessParamsResult.innerException);
            }
            
            // if ProcessParameters succeeded, parameters contains all of the arguments for the command, converted to the right types. this means that we finnally call the command.
            var returnValue = command.Invoke(this, parameters);

            Service.lastResult = returnValue;
        }

        public MethodInfo GetCommand(string commandName, string[] args)
        {
            // get all methods on this object
            var commands = this.GetType().GetMethods().AsEnumerable();

            // filter out non command methods like ToString and Equals
            commands = commands.Where(m => m.DeclaringType != typeof(Service) && m.DeclaringType != typeof(object));

            if (commands.Count() == 0)
                throw new Exception("The service \"" + this.GetType().Name + "\" has no commands!");

            // find the all of the commands with the right name
            commands = commands.Where(m => m.Name.ToLower() == commandName.ToLower());

            if (commands.Count() == 0)
                throw new Exception("Unrecognized command");

            // finally, filter by arg count
            commands = commands.Where(m => m.GetParameters().Count() == args.Count());

            if (commands.Count() == 0)
                throw new Exception("Incorrect argument count");

            // if there is more than one command that matches, we dont have enough information to determine the right one to call.
            if (commands.Count() > 1)
            {
                // format exception string
                string exString = "Ambiguous reference to the following commands:\n";

                foreach (var c in commands)
                {
                    exString += "   " + this.GetType().Name + ":" + c.Name;

                    foreach (var p in c.GetParameters())
                    {
                        exString += " " + p.ParameterType.Name;
                    }

                    exString += '\n';
                }

                throw new Exception(exString);
            }

            return commands.First();
        }

        public bool TryGetCommand(string commandName, string[] args, out MethodInfo result)
        {
            result = null;

            // get all methods on this object
            var commands = this.GetType().GetMethods().AsEnumerable();

            // filter out non command methods like ToString and Equals
            commands = commands.Where(m => m.DeclaringType != typeof(Service) && m.DeclaringType != typeof(object));

            if (commands.Count() == 0)
                return false;

            // find the all of the commands with the right name
            commands = commands.Where(m => m.Name.ToLower() == commandName.ToLower());

            if (commands.Count() == 0)
                return false;

            // finally, filter by arg count
            commands = commands.Where(m => m.GetParameters().Count() == args.Count());

            if (commands.Count() != 1)
                return false;

            // if there is more than one command that matches, we dont have enough information to determine the right one to call.

            result = commands.First();
            return true;
        }

        private (int index, Exception innerException)  ProcessParameters(MethodInfo command, object[] parameters, string[] args)
        {
            int i = 0;
            ParameterInfo p;

            void Parse<T>(Func<string, T> parseFunc)
            {
                parameters[i] = parseFunc(args[i]);
            }

            // see if there are any numeric parameters we can convert to
            try
            {
                for (i = 0; i < parameters.Length; i++)
                {
                    p = command.GetParameters()[i];
                    switch (p.ParameterType.Name)
                    {
                        case nameof(Boolean):
                            Parse(bool.Parse);
                            break;
                        case nameof(SByte):
                            Parse(sbyte.Parse);
                            break;
                        case nameof(Byte):
                            Parse(byte.Parse);
                            break;
                        case nameof(Int16):
                            Parse(short.Parse);
                            break;
                        case nameof(UInt16):
                            Parse(ushort.Parse);
                            break;
                        case nameof(Int32):
                            Parse(int.Parse);
                            break;
                        case nameof(UInt32):
                            Parse(uint.Parse);
                            break;
                        case nameof(Int64):
                            Parse(long.Parse);
                            break;
                        case nameof(UInt64):
                            Parse(ulong.Parse);
                            break;
                        case nameof(Single):
                            Parse(float.Parse);
                            break;
                        case nameof(Double):
                            Parse(double.Parse);
                            break;
                        case nameof(Decimal):
                            Parse(decimal.Parse);
                            break;
                        case nameof(Char):
                            Parse(char.Parse);
                            break;
                        case nameof(String):
                            parameters[i] = args[i];
                            break;
                        default:
                            return (i, new Exception("Cannot have non-primitive argument"));
                    }
                }
            }
            catch(Exception ex)
            {
                return (i, ex);
            }

            return (i, null);
        }
    }
}
