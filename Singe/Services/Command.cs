using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Services
{
    internal sealed class Command
    {
        static Type[] ProcessParameters(ParameterInfo[] parameters, out int lastResultIndex)
        {
            if(parameters.Length == 0)
            {
                lastResultIndex = -1;
                return new Type[0];
            }

            var result = new List<Type>(parameters.Length - 1);
            lastResultIndex = -1;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].GetCustomAttribute<LastResultAttribute>() != null)
                {
                    lastResultIndex = i;
                }
                else
                {
                    result.Add(parameters[i].ParameterType);
                } 
            }

            return result.ToArray();
        }

        public Command(MethodInfo info) : this(info.ReturnType, info.GetCustomAttribute<CommandAttribute>().serviceName.ToLower(), info.Name.ToLower(), ProcessParameters(info.GetParameters(), out int lastResultIndex), lastResultIndex, info)
        {
        }

        public Command(MethodInfo info, string serviceName) : this(info.ReturnType, serviceName.ToLower(), info.Name.ToLower(), ProcessParameters(info.GetParameters(), out int lastResultIndex), lastResultIndex, info)
        {
        }

        public Command(Type returnType, string nameSpace, string name, Type[] parameters, int lastResultIndex, MethodInfo methodInfo)
        {
            ReturnType = returnType;
            Signature = new CommandSignature(nameSpace, name, new List<Type>(parameters));
            LastResultIndex = lastResultIndex;
            MethodInfo = methodInfo;
        }

        /// <summary>
        /// The type of object returned by the command.
        /// </summary>
        public Type ReturnType { get; private set; }

        public CommandSignature Signature { get; private set; }

        /// <summary>
        /// The index in the argument array where the return value of the last command should be inserted.
        /// </summary>
        public int LastResultIndex { get; private set; }

        /// <summary>
        /// A <see cref="System.Reflection.MethodInfo"/> for the method the command maps to.
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }

        /// <summary>
        /// Invokes the command with the specified last result and arguments.
        /// </summary>
        /// <param name="lastResult"></param>
        /// <param name="args"></param>
        /// <returns>The object returned by the command.</returns>
        public object Invoke(object lastResult, object[] args)
        {
            var argsWithLastResult = new List<object>(args); 

            if (LastResultIndex > -1)
            {

                if (lastResult != null && !MethodInfo.GetParameters()[LastResultIndex].ParameterType.IsAssignableFrom(lastResult.GetType()))
                {
                    lastResult = null;
                }

                argsWithLastResult.Insert(LastResultIndex, lastResult);
            }

            var parameters = MethodInfo.GetParameters();
            if(argsWithLastResult.Count < parameters.Length)
            {
                for (int i = argsWithLastResult.Count; i < parameters.Length; i++)
                {
                    argsWithLastResult.Add(parameters[i].DefaultValue);
                }
            }

            int argsIndex;

            void Parse<T>(Func<string, T> parse)
            {
                argsWithLastResult[argsIndex] = parse(argsWithLastResult[argsIndex].ToString());
            }

            for (argsIndex = 0; argsIndex < parameters.Length; argsIndex++)
            {
                if (parameters[argsIndex].ParameterType != typeof(string) && argsIndex != LastResultIndex)
                {
                    switch (parameters[argsIndex].ParameterType.Name)
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
                            Parse(s => s);
                            break;
                        default:
                            break;
                    }
                }
            }

            return MethodInfo.Invoke(null, argsWithLastResult.ToArray());
        }
    }
}
