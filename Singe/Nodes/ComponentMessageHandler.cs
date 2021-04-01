using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe
{
    delegate void Init();
    delegate void Update(float dt);
    delegate void Destroy();

    internal sealed class ComponentMessageHandler
    {
        public Type ComponentType { get; }
        public Component Component { get; }

        public Init Init { get; }
        public Update Update { get; }
        public Destroy Destroy { get; }

        public ComponentMessageHandler(Component component)
        {
            this.Component = component;

            ComponentType = component.GetType();

            Init = CreateDelegateForMethod<Init>();
            Update = CreateDelegateForMethod<Update>();
            Destroy = CreateDelegateForMethod<Destroy>();

        }


        private T CreateDelegateForMethod<T>() where T : Delegate
        {
            return (T)Delegate.CreateDelegate(typeof(T), this.Component, typeof(T).Name);
        }
    }
}
