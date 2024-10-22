using ToyAnalyzer.Lexer;
using ToyAnalyzer.Parser;

namespace ToyAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var filename))
        {
            return;
        }

        var source = filename != null ? File.ReadAllText(filename) : Console.In.ReadToEnd();
        var parser = CreateParser(source);

        try
        {
            var xml = parser.Parse();
            xml.Save(Console.Out);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Parsing failed!");
            Console.WriteLine(ex.Message);
        }
    }

    private static bool TryParseArgs(string[] args, out string? filename)
    {
        if (args.Length == 0)
        {
            filename = null;
            return true;
        }

        if (args.Length > 1)
        {
            Console.WriteLine("Too many arguments!");
            filename = null;
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

    private static Parser.Parser CreateParser(string source)
    {
        var rules = GrammarConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.grammar_rules.json");
        var table = ParserTableGenerator.GenerateTable(rules);
        var lexer = CreateLexer(source);
        return new Parser.Parser(lexer, table);
    }

    private static Lexer.Lexer CreateLexer(string source)
    {
        var rules = LexerConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.lexer_rules.json");
        return new Lexer.Lexer(source, rules);
    }
}
