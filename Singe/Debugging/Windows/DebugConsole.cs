using Commander;
using ImGuiNET;
using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Singe.Debugging.Windows
{
    [MessageListener]
    public static class DebugConsole
    {
        private static List<ConsoleTextBlock> entries = new List<ConsoleTextBlock>();
        static bool open;
        static string textInput = "";
        static IConsole console = new DebugConsoleOutput();


        static DebugConsole()
        {
            Input.BindCommandToKey(Key.F1, "Console:ToggleConsole");
        }

        [Command("Console")]
        public static void ToggleConsole()
        {
            open = !open;
        }

        [Command("Console")]
        public static void Color(string color)
        {
            Color c = System.Drawing.Color.FromName(color.ToLower());

            if(c.A != 0)
            {
                console.Color = c;

                console.WriteLine("Color set to " + c.Name);
            }
            else
            {
                console.WriteLine("Unknown color \"" + color + "\"");
            }

        }

        [Command]
        public static void Clear()
        {
            entries = new List<ConsoleTextBlock>();
        }

        public static void OnGui()
        {
            if(open)
            {
                if (ImGui.Begin("Console", ref open, ImGuiWindowFlags.NoScrollbar))
                {
                    ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5.0f);
                    ImGui.BeginChild("ConsoleText", new Vector2(0, -20), true, ImGuiWindowFlags.AlwaysVerticalScrollbar);

                    for (int i = 0; i < entries.Count; i++)
                    {
                        entries[i].OnGui();
                    }

                    ImGui.EndChild();
                    ImGui.PopStyleVar();

                    ImGui.Text(">");
                    ImGui.SameLine();

                    if (ImGui.InputText("", ref textInput, 32, ImGuiInputTextFlags.EnterReturnsTrue))
                    {
                        (console as DebugConsoleOutput).WriteLine(">" + textInput, true);
                        Service.SubmitCommandString(textInput);
                        textInput = "";
                    }

                    ImGui.End();
                }
            }
        }
        public static IConsole GetConsole()
        {
            return console;
        }

        private class DebugConsoleOutput : IConsole
        {
            public DebugConsoleOutput()
            {
                Service.Output = this;
            }

            public Color Color { get; set; } = Color.White;

            public void WriteLine(object text, bool separator)
            {
                Write(text.ToString() + '\n', separator);
            }

            public void Write(object text) => Write(text, false);

            public void Write(object obj, bool separator)
            {
                var str = obj.ToString();

                StringReader reader = new StringReader(str);

                var t = reader.ReadLine();
                entries.Add(new ConsoleTextBlock(t ?? "", new Vector4((float)Color.R, (float)Color.G, (float)Color.B, 255), str.Contains('\n'), separator));
            }
        }

        private unsafe class ConsoleTextBlock
        {
            public Vector4 color;
            public bool newLine;
            public bool separator;
            public string text;

            public ConsoleTextBlock(string text, Vector4 color, bool newLine, bool separator)
            {
                this.text = text;
                this.color = color;
                this.newLine = newLine;
                this.separator = separator;
            }

            public void OnGui()
            {
                if(separator)
                {
                    ImGui.Separator();
                }
                ImGui.TextColored(color, text);

                if (!newLine)
                {
                    ImGui.SameLine();
                }
                if (separator)
                {
                    ImGui.Separator();
                }
            }
        }
    }
}
