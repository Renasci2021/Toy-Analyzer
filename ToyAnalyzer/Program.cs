using ToyAnalyzer.Lexer;

namespace ToyAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var filename)) return;

        var source = File.ReadAllText(filename);
        var lexer = CreateLexer(source);

        ProcessTokens(lexer);
    }

    private static bool TryParseArgs(string[] args, out string filename)
    {
        filename = string.Empty;

        if (args.Length != 1)
        {
            Console.WriteLine("Usage: ToyAnalyzer <source file>");
            return false;
        }

        filename = args[0];

        if (!File.Exists(filename))
        {
            Console.WriteLine($"File not found: {filename}");
            return false;
        }

        return true;
    }

    private static Lexer.Lexer CreateLexer(string source)
    {
        var rules = LexerConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.lexer_rules.json");
        return new Lexer.Lexer(source, rules);
    }

    private static void ProcessTokens(Lexer.Lexer lexer)
    {
        while (true)
        {
            var token = lexer.NextToken();

            if (token.Type == "EOF")
            {
                break;
            }

            Console.WriteLine($"('{token.Type}', '{token.Value}')");
        }
    }
}
