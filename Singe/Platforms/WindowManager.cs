using Singe.Platforms.Implementations.Windows;
using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Singe.Platforms
{
    public abstract class WindowManager : IRenderingOutputFactory, IDisposable
    {
        #region statics
        public static WindowManager Create()
        {
            return new HwndManager();
        }
        #endregion

        public abstract void HandleEvents();

        public abstract void SetWindowMode(WindowMode mode);

        public abstract bool RequestSizeChange(Size newSize);
        public abstract bool RequestPositionChange(Point newPosition);
        public abstract bool RequestTitleChange(string title);

        public abstract Size GetSize();
        public abstract Point GetPosition();
        public abstract string GetTitle();

        public abstract InputDevice CreateInputDevice();

        public abstract GraphicsApi[] GetSupportedApis();

        public abstract DisplayInformation[] GetDisplayInformation();

        public abstract event EventHandler<SizeChangedEventArgs> SizeChanged;
        public abstract event EventHandler<PositionChangedEventArgs> PositionChanged;
        public abstract event EventHandler<TitleChangedEventArgs> TitleChanged;

        public abstract void SetMousePos(Vector2 point);

        public abstract void Dispose();
        public abstract IRenderingOutput CreateOutput(Renderer renderer);
    }
}
