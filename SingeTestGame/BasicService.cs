using Singe;
using Singe.Services;
using System;
using System.Collections.Generic;

namespace SingeTestGame
{
    class BasicService
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Service.SubmitCommandString(Console.ReadLine());
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

        [Command]
        public void Say(string words, int count)
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine(words);
        }
    }

    class HelloService
    {
        public void SetColor(string color)
        {
            Console.BackgroundColor = Enum.Parse<ConsoleColor>(color);
        }
    }
}
