using Singe.Platforms;
using Singe.Services;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Singe
{
    public static class Input
    {
        private static List<Key> pressedKeys;
        private static List<Key> downKeys;
        private static List<Key> upKeys;

        private static List<char> typedChars;

        private static InputDevice inputDevice;

        private static Vector2 mousePosition;

        private static int delta;

        static Input()
        {
            pressedKeys = new List<Key>();
            downKeys = new List<Key>();
            upKeys = new List<Key>();
            typedChars = new List<char>();
        }

        internal static void Update()
        {
            downKeys.Clear();
            upKeys.Clear();
            typedChars.Clear();
            delta = 0;
        }

        internal static void SetDevice(InputDevice device)
        {
            inputDevice = device;

            inputDevice.KeyDown += InputDevice_KeyDown;
            inputDevice.KeyUp += InputDevice_KeyUp;
            inputDevice.MouseMoved += InputDevice_MouseMoved;
            inputDevice.CharTyped += InputDevice_CharTyped;
            inputDevice.Scrolled += InputDevice_Scrolled;
        }

        private static void InputDevice_Scrolled(object sender, ScrollEventArgs e)
        {
            delta = e.Delta;
        }

        private static void InputDevice_CharTyped(object sender, CharEventArgs e)
        {
            typedChars.Add(e.Character);
        }

        private static void InputDevice_MouseMoved(object sender, MouseEventArgs e)
        {
            mousePosition = e.Position;
        }

        private static void InputDevice_KeyUp(object sender, KeyEventArgs e)
        {
            upKeys.Add(e.Key);

            if(pressedKeys.Contains(e.Key))
            {
                pressedKeys.Remove(e.Key);
            }
        }

        private static void InputDevice_KeyDown(object sender, KeyEventArgs e)
        {
            downKeys.Add(e.Key);

            if (pressedKeys.Contains(e.Key))
                return;

            pressedKeys.Add(e.Key);
        }

        public static Vector2 GetMousePosition()
        {
            return mousePosition;
        }

        public static bool GetKey(Key key)
        {
            return pressedKeys.Contains(key);
        }

        public static bool GetKeyDown(Key key)
        {
            return downKeys.Contains(key);
        }

        public static bool GetKeyUp(Key key)
        {
            return upKeys.Contains(key);
        }

        public static char[] GetTypedChars()
        {
            return typedChars.ToArray();
        }

        public static int GetScrollDelta()
        {
            return delta;
        }

        [Command("input")]
        public static void SendChar(char c)
        {
            InputDevice_CharTyped(null, new CharEventArgs(c));
        }
    }
}
