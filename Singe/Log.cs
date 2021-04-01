using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Singe
{
    public static class Log
    {
        public static void WriteLine(object text)
        {
            Debugging.Windows.DebugConsole.GetConsole().WriteLine(text);
        }

        public static void WriteLine(object text, Color color)
        {
            Debugging.Windows.DebugConsole.GetConsole().WriteLine(text, color);
        }

        public static void Warn(object text)
        {
            Debugging.Windows.DebugConsole.GetConsole().WriteLine(text, Color.Yellow);
        }

        public static Exception Error(object text)
        {
            Debugging.Windows.DebugConsole.GetConsole().WriteLine(text.ToString(), Color.Red);
            return new Exception(text.ToString());
        }
    }
}
