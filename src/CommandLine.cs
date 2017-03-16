using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;

namespace make
{
    public class CommandLine
    {
        public string Program { get; set; }
        public string OutputFile { get; set; }
        public string InputFile { get; set; }
        public string[] Arguments { get; set; }


        private CommandLine()
        {
        }

        public static CommandLine Parse(string[] args)
        {
            var commandLine = new CommandLine();

            var options = new OptionSet
            {
                { "p|program=", v => commandLine.Program = v },
                { "o|out=", v => commandLine.OutputFile = v },
            };

            try
            {
                var remaining = options.Parse(args);
                commandLine.ParseRemainingArguments(remaining);
            }
            catch (OptionException e)
            {
                Console.Error.WriteLine(e.Message);
            }

            return commandLine;
        }

        private void ParseRemainingArguments(List<string> remaining)
        {
            var input = "";
            var options = new List<string>();

            foreach (var arg in remaining)
            {
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                    options.Add(arg);
                else if (File.Exists(arg))
                    input = arg;
                else
                    options.Add(arg);
            }

            InputFile = input;
            Arguments = options.ToArray();
        }
    }
}