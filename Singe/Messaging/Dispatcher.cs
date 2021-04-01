using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Messaging
{
    public delegate void Message();

    public class Dispatcher
    {
        Dictionary<MessageType, List<Message>> messages;
        private object currentArgument;

        public Dispatcher()
        {
            messages = new Dictionary<MessageType, List<Message>>();

            var listeners = AssemblySearch.GetTypesWithAttribute<MessageListenerAttribute>();

            foreach (var listener in listeners)
            {
                var messageNames = Enum.GetValues(typeof(MessageType));

                foreach (var messageType in messageNames)
                {
                    var type = (MessageType)messageType;
                    
                    if (!messages.ContainsKey(type))
                        messages.Add(type, new List<Message>());

                    var m = listener.GetMethod(messageType.ToString(), BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);

                    if (m == null)
                    {
                        continue;
                    }

                    if (m.GetParameters().Length != 0 || m.ReturnType != typeof(void))
                    {
                        continue;
                    }

                    try
                    {
                        var d = (Message)m.CreateDelegate(typeof(Message));
                        messages[type].Add(d);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public void BroadcastMessage(MessageType type, object argument)
        {
            currentArgument = argument;

            if (messages.ContainsKey(type))
                foreach (var d in messages[type])
                {
                    d.Invoke();
                }

            currentArgument = null;
        }

        public object GetArgument()
        {
            return currentArgument;
        }

        public T GetArgument<T>()
        {
            return (T)GetArgument();
        }

    }
}
