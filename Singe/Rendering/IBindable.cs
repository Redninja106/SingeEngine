using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public interface IBindable
    {
        BindableType GetBindableType();
        void OnBind(ObjectBinder binder);
        void OnUnbind(ObjectBinder binder);
    }
}
