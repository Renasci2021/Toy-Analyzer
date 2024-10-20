using ToyAnalyzer.Lexer;
using ToyAnalyzer.Parser;

namespace ToyAnalyzer;

class Program
{
    readonly static string source = @"

var x;
var y;
var z;
input x;
z = -5;
if (x > 5) {
   y = x * (x / 2 + 10) - z;
}
print ""After if, finished!"";

    ";

    static void Main(string[] args)
    {
        // if (!TryParseArgs(args, out var filename)) return;
        // var source = File.ReadAllText(filename);
        var parser = CreateParser(source);
        parser.Parse();
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

    private static Parser.Parser CreateParser(string source)
    {
        var rules = GrammarConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.grammar_rules.json");
        foreach (var rule in rules.Values)
        {
            rule.ComputeFirstSet(rules);
            rule.ComputeFollowSet(rules, "PROGRAM");
        }
        var table = ParserTableGenerator.Generate(rules);

        var lexer = CreateLexer(source);
        return new Parser.Parser(lexer, rules, table);
    }

    private static Lexer.Lexer CreateLexer(string source)
    {
        var rules = LexerConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.lexer_rules.json");
        return new Lexer.Lexer(source, rules);
    }
}
