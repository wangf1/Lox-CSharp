
namespace Lox_CSharp.CraftingInterpreters.tool
{
    internal class GenerateAst
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                System.Console.Error.WriteLine("Usage: GenerateAst <output directory>");
                System.Environment.Exit(64);
            }

            string outputDir = args[0];

            DefineAst(outputDir, "Expr",
                [
                    "Binary   : Expr left, Token operator, Expr right",
                    "Grouping : Expr expression",
                    "Literal  : object value",
                    "Unary    : Token operator, Expr right"
                ]);
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            String path = outputDir + "/" + baseName + ".cs";
            using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("");
                writer.WriteLine("namespace Lox_CSharp.CraftingInterpreters");
                writer.WriteLine("{");
                writer.WriteLine($"    internal abstract class {baseName}");
                writer.WriteLine("    {");
                foreach (string s in types)
                {
                    //TODO
                }
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
    }
}

