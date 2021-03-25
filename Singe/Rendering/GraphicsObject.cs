using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class GraphicsObject : IDisposable
    {
        public string DebugName { get => GetDebugName(); set => SetDebugName(value); }

        private List<IDisposable> disposables;
        private string debugName;

        public GraphicsObject()
        {
            disposables = new List<IDisposable>();
        }

        private protected void RegisterDisposableObject(IDisposable disposableObject)
        {
            disposables.Add(disposableObject);
        }

        private protected void UnregisterDisposableObject(IDisposable disposableObject)
        {
            UnregisterDisposableObject(disposableObject, false);
        }

        private protected void UnregisterDisposableObject(IDisposable disposableObject, bool dispose)
        {
            disposables.Remove(disposableObject);

            if(dispose)
            {
                disposableObject.Dispose();
            }
        }

        public virtual void Dispose()
        {
            foreach (var d in disposables)
            {
                try
                {
                    d.Dispose();
                    disposables.Remove(d);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private protected virtual void SetDebugName(string name)
        {
            debugName = name;
        }

        private protected virtual string GetDebugName()
        {
            return debugName;
        }
    }
}
