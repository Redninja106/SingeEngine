using Singe;
using Singe.Services;
using System;
using System.Collections.Generic;

namespace SingeTestGame
{
    class BasicService : Service
    {
        static void Main(string[] args)
        {
            var s = new HelloService();
            var p = new BasicService();
            while (true)
            {
                SubmitCommand(Console.ReadLine());
            }
        }

        public void SetColor(string color)
        {
            Console.ForegroundColor = Enum.Parse<ConsoleColor>(color);
        }

        public void Run()
        {
            Application.Run(Singe.Rendering.RenderingMode.Immediate);
        }
    }

    class HelloService : Service
    {
        public void SetColor(string color)
        {
            Console.BackgroundColor = Enum.Parse<ConsoleColor>(color);
        }
    }
}
