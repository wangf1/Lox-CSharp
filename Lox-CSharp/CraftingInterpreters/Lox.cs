using System;
using System.IO;
using System.Text;

namespace Lox_CSharp.CraftingInterpreters
{
    public class Lox
    {
        private static bool HadError = false;

        public static void Main(string[] args)
        {
            switch (args.Length)
            {
                case > 1:
                    Console.WriteLine("Usage: lox [script]");
                    Environment.Exit(64);
                    break;
                case 1:
                    RunFile(args[0]);
                    break;
                default:
                    RunPrompt();
                    break;
            }
        }

        private static void RunPrompt()
        {
            using var reader = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                Console.Write("> ");
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                Run(line);
                HadError = false;
            }
        }

        private static void RunFile(string path)
        {
            var source = File.ReadAllText(path, Encoding.Default);
            Run(source);
        }

        private static void Run(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        internal static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            HadError = true;
        }
    }
}
