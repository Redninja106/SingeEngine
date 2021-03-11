using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Singe.Rendering
{
    public abstract class GraphicsResource : IDisposable
    {
        public string DebugName { get; set; }

        private List<IDisposable> disposables;

        public GraphicsResource()
        {
            disposables = new List<IDisposable>();
        }

        private protected void RegisterDisposableObject(IDisposable disposableObject)
        {
            disposables.Add(disposableObject);
        }

        private protected void UnregisterDisposableObject(IDisposable disposableObject)
        {
            disposables.Remove(disposableObject);
        }

        private protected void DisposeDisposableObject(IDisposable disposableObject)
        {
            UnregisterDisposableObject(disposableObject);
            disposableObject.Dispose();
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
    }
}
