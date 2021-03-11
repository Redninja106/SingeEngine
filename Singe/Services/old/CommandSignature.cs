using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Services.old
{
    class CommandSignature
    {
        public Type ReturnType { get; private set; }
        public string ServiceName { get; private set; } 
        public string CommandName { get; private set; } 
        public Type[] ParameterTypes { get; private set; }
        
        public CommandSignature(string signature)
        {
            
        }

        public CommandSignature(Type returnType, string serviceName, string commandName, Type[] parameterTypes)
        {
            ReturnType = returnType;
            ServiceName = serviceName;
            CommandName = commandName;
            ParameterTypes = parameterTypes;
        }

        public CommandSignature(string serviceName, MethodInfo methodInfo)
        {
            ReturnType = methodInfo.ReturnType;
            ServiceName = serviceName;
            CommandName = methodInfo.Name;

            var paramInfos = methodInfo.GetParameters();
            ParameterTypes = new Type[paramInfos.Length];

            for (int i = 0; i < ParameterTypes.Length; i++)
            {
                ParameterTypes[i] = paramInfos[i].ParameterType;
            }
        }

        public override string ToString()
        {
            string result = $"[{ReturnType.FullName} {ServiceName}(";

            for (int i = 0; i < ParameterTypes.Length; i++)
            {
                if (i != 0)
                    result += ", ";

                result += ParameterTypes[i].FullName;
            }
            
            result += ")]";
            
            return result;
        }
    }
}
