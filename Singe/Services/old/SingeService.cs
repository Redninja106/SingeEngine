using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Services.old
{
    public class SingeService : Service
    {
        public void Say(string words, byte times)
        {
            for (int i = 0; i < times; i++)
            {
                Console.WriteLine(words);
            }
        }

        public void Say(string words)
        {
            Console.WriteLine(words);
        }

        public string SetStr(string str)
        {
            return str;
        }

        public void PrintStr()
        {
            Console.WriteLine(GetLastCommandResult());
        }

        public void Exit()
        {
            Exit(0);
        }

        public void Exit(int code)
        {
            Application.Exit(code);
        }
    }
}
