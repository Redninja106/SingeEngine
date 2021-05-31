using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Rendering
{
    public static class BindingExtensions
    {
        public static void UnbindAll(this ObjectBinder objectBinder)
        {
            foreach (var bindable in objectBinder.GetBoundObjects())
            {
                objectBinder.BindObject(bindable);
            }
        }

        public static void UnbindAll<T>(this ObjectBinder objectBinder) where T : IBindable
        {
            foreach (var bindable in objectBinder.GetBoundObjects().Where(b => b.GetType() == typeof(T)))
            {
                objectBinder.UnbindObject(bindable);
            }
        }

        public static void UnbindAll(this ObjectBinder objectBinder, BindableType type)
        {
            objectBinder.UnbindAll(b => b.GetBindableType() == type);
        }

        public static void UnbindAll<T>(this ObjectBinder objectBinder, Predicate<T> condition) where T : IBindable
        {
            foreach (var bindable in objectBinder.GetBoundObjects())
            {
                if (bindable.GetType() != typeof(T))
                    continue;

                if (!condition((T)bindable))
                    continue;

                objectBinder.BindObject(bindable);
            }
        }

        public static void UnbindAll(this ObjectBinder objectBinder, Predicate<IBindable> condition)
        {
            var objects = objectBinder.GetBoundObjects();
            var enumerator = objects.GetEnumerator();
            List<IBindable> toUnbind = new List<IBindable>();
            while(enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                    toUnbind.Add(enumerator.Current);
            }

            foreach (var bindable in toUnbind)
            {
                objectBinder.UnbindObject(bindable);
            }
        }

        public static IBindable GetBoundObject(this ObjectBinder objectBinder, BindableType bindableType)
        {
            return objectBinder.GetBoundObjects().Where(b => b.GetBindableType() == bindableType).FirstOrDefault();
        }
    }
}
