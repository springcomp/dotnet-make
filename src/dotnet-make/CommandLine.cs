using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            // canonicalizing paths

            var input = "";
            var options = new List<string>();
            var arguments = remaining.AsEnumerable();

            if (Program == null)
            {
                Program = remaining.FirstOrDefault(a => !a.StartsWith("/") && !a.StartsWith("/"));
                if (Program == null)
                {
                    Console.Error.WriteLine("Wrong argument count. Please, use the -t switch to specify a valid program name.");
                    Environment.Exit(1);
                }
                Program = Path.GetFullPath(Program);
                arguments = remaining.Skip(1);
            }

            foreach (var arg in arguments)
            {
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                    options.Add(arg);
                else if (File.Exists(arg))
                    input = arg;
                else
                    options.Add(arg);
            }

            InputFile = Path.GetFullPath(input);
            OutputFile = Path.GetFullPath(OutputFile);
            Arguments = options.ToArray();
        }
    }
}