using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public class ObjectBinder
    {
        List<IBindable> boundObjects = new List<IBindable>();
        public void BindObject(IBindable bindableObject)
        {
            if (bindableObject == null)
                return;

            boundObjects.Add(bindableObject);
            bindableObject.OnBind(this);
        }
        public void UnbindObject(IBindable bindableObject)
        {
            if (bindableObject == null)
                return;
            
            boundObjects.Remove(bindableObject);
            bindableObject.OnUnbind(this);
        }
        public IEnumerable<IBindable> GetBoundObjects()
        {
            return boundObjects;
        }
    }
}
