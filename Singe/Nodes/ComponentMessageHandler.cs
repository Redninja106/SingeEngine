using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe
{
    delegate void Start();
    delegate void Update(float dt);

    internal sealed class ComponentMessageHandler
    {
        public Type ComponentType { get; }
        public Component Component { get; }

        public Start Start { get; }
        public Update Update { get; }

        public ComponentMessageHandler(Component component)
        {
            this.Component = component;

            ComponentType = component.GetType();

            Start = CreateDelegateForMethod<Start>();
            Update = CreateDelegateForMethod<Update>();
            
        }


        private T CreateDelegateForMethod<T>() where T : Delegate
        {
            return (T)Delegate.CreateDelegate(typeof(T), this.Component, typeof(T).Name);
        }
    }
}
