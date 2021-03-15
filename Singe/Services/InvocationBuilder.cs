using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe.Services.Parsing
{
    internal class InvocationBuilder
    {
        StringParser reader;
        List<CommandInvocation> invocations;
        CommandInvocation current;

        public InvocationBuilder()
        {

        }

        public List<CommandInvocation> GetInvocations(string reqString)
        {
            reader = new StringParser(reqString);
            invocations = new List<CommandInvocation>();
            current = CommandInvocation.Default;

            while (!reader.IsAtEnd)
            {
                reader.Next();
                HandleChar();
            }

            if(!current.Equals(CommandInvocation.Default))
            {
                NewCommand();
            }

            return invocations;
        }

        void NewCommand()
        {
            invocations.Add(current);

            current = CommandInvocation.Default;
        }

        void HandleChar()
        {
            if (current.Name == null)
            {
                reader.ReadWhile(' ');
                if (char.IsLetter(reader.Current) || reader.Current == '_')
                {
                    var s = reader.ReadWhile(() => char.IsLetterOrDigit(reader.Current) || reader.Current == '_');
                    if (reader.Current == ' ' || reader.Current == (char)0)
                    {
                        if (current.Service == null)
                        {
                            current.Service = "";
                        }
                        current.Name = s.ToLower();
                    }
                    else if (reader.Current == ':')
                    {
                        current.Service = s.ToLower();
                        reader.Next();
                        current.Name = reader.ReadWhile(() => char.IsLetterOrDigit(reader.Current) || reader.Current == '_');
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (reader.Current == (char)0)
                {
                    NewCommand();
                    return;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                var s = reader.ReadUntil('|', '"', StringParser.End);

                current.Parameters.AddRange(s.Split());
                current.Parameters.RemoveAll(o => o.ToString() == "");

                if (reader.Current == '"')
                {
                    reader.Next();
                    current.Parameters.Add(reader.ReadUntil('"', StringParser.End));
                }
                else
                {
                    NewCommand();
                }
            }
        }
    }
}
