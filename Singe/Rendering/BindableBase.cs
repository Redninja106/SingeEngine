using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public abstract class BindableBase : IBindable
    {
        public bool IsBound { get; private set; }
        public virtual BindableType GetBindableType()
        {
            return BindableType.Other;
        }

        public virtual void OnBind(ObjectBinder binder)
        {
            IsBound = true;
        }

        public virtual void OnUnbind(ObjectBinder binder)
        {
            IsBound = false;
        }
    }
}
