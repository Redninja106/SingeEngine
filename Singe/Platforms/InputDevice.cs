using System;
using System.Numerics;

namespace Singe.Platforms
{
    public sealed class InputDevice
    {
        /// <summary>
        /// Gets the windowManager which this <see cref="InputDevice"/> belongs to.
        /// </summary>
        public WindowManager Window { get; }

        /// <summary>
        /// Creates a new <see cref="InputDevice"/>.
        /// </summary>
        /// <param name="window">The window which this <see cref="InputDevice"/> belongs too.</param>
        public InputDevice(WindowManager window)
        {
            this.Window = window;
        }

        /// <summary>
        /// Occurs when a key is released.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a key is OS sends a char to the application. 
        /// </summary>
        public event EventHandler<CharEventArgs> CharTyped;

        /// <summary>
        /// Occurs when the mouse wheel is moved.
        /// </summary>
        public event EventHandler<ScrollEventArgs> Scrolled;

        /// <summary>
        /// Occurs when the mouse is moved.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseMoved;

        /// <summary>
        /// Invokes the <see cref="KeyUp"/> event with the provided key as it's arguments.
        /// </summary>
        /// <param name="key">The key that was released.</param>
        public void OnKeyUp(Key key)
        {
            KeyUp?.Invoke(this, new KeyEventArgs(key));
        }

        /// <summary>
        /// Invokes the <see cref="KeyDown"/> event with the provided key as it's arguments.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public void OnKeyDown(Key key)
        {
            KeyDown?.Invoke(this, new KeyEventArgs(key));
        }

        /// <summary>
        /// Invokes the <see cref="CharTyped"/> event with the provided key as it's arguments.
        /// </summary>
        /// <param name="c">The key that was typed.</param>
        public void OnKeyTyped(char c)
        {
            CharTyped?.Invoke(this, new CharEventArgs(c));
        }

        /// <summary>
        /// Invokes the <see cref="Scrolled"/> event with the provided mouse wheel delta as it's arguments.
        /// </summary>
        /// <param name="delta">The mouse wheel delta.</param>
        public void OnScroll(int delta)
        {
            Scrolled?.Invoke(this, new ScrollEventArgs(delta));
        }

        /// <summary>
        /// Invokes the <see cref="MouseMoved"/> event with the provided mouse position as it's arguments.
        /// </summary>
        /// <param name="position">The mouse position.</param>
        public void OnMouseMove(Vector2 position)
        {
            MouseMoved?.Invoke(this, new MouseEventArgs(position));
        }
    }
}
