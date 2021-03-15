using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Services
{
    public static class SingeServices
    {
        [Command]
        public static void Say(string words, int count = 1)
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine(words);
        }

        [Command("Singe")]
        public static void Exit(int code = 0)
        {
            Exit(code);
        }

        [Command("Singe")]
        public static string GetText(string text)
        {
            return text;
        }

        [Command("Singe")]
        public static void PrintText([LastResult] string text)
        {
            Console.WriteLine(text);
        }
    }
}
