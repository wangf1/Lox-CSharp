using System;
using System.IO;
using System.Text;

namespace Lox_CSharp.CraftingInterpreters
{
    public class Lox
    {
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

        static void error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
        }
    }
}
