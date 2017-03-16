using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace make
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var commandLine = CommandLine.Parse(args);

            var echo = new StringBuilder();
            echo.AppendFormat("{0} ", commandLine.Program);
            foreach (var arg in commandLine.Arguments)
                echo.AppendFormat("{0} ", arg);
            if (!String.IsNullOrEmpty(commandLine.OutputFile))
                echo.AppendFormat("/out:{0} ", commandLine.OutputFile);
            if (!String.IsNullOrEmpty(commandLine.InputFile))
                echo.Append(commandLine.InputFile);

            Console.WriteLine(echo);

            var output = File.Exists(commandLine.OutputFile)
                ? File.GetLastWriteTimeUtc(commandLine.OutputFile)
                : DateTime.MinValue
                ;

            var input = File.Exists(commandLine.InputFile)
                ? File.GetLastWriteTimeUtc(commandLine.InputFile)
                : DateTime.MaxValue
                ;

            if (input > output)
            {
                Console.WriteLine($"Building {commandLine.OutputFile} because {commandLine.InputFile} is more recent...");
                var process = Process.Start("dotnet", echo.ToString());
                process.WaitForExit();
            }
        }
    }
}
