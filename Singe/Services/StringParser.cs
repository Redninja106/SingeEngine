using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Services.Parsing
{
    internal sealed class StringParser
    {
        public const char End = (char)0;

        public bool IsAtEnd => Current == End;

        public int Position { get; private set; }
        public char Current { get; private set; }
        public string String { get; private set; }

        public StringParser(string str)
        {
            Position = -1;
            Current = (char)1;
            this.String = str + End;
        }

        public char Next()
        {
            return Current = String[++Position];
        }

        public char Peek()
        {
            return String[Current];
        }

        public string ReadUntil(params char[] chars)
        {
            return ReadWhile(()=>chars.All(c => Current != c));
        }

        public string ReadWhile(Func<bool> condition)
        {
            string s = "";
            while (condition())
            {
                s += Current;
                Next();
            }
            return s;
        }

        public string ReadWhile(params char[] chars)
        {
            return ReadWhile(()=>chars.Any(c=>Current==c));
        }
    }
}
