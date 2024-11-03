using ToyAnalyzer.Lexer;
using ToyAnalyzer.Parser.Common;

namespace ToyAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var filename, out var parserType))
        {
            return;
        }

        var source = filename != null ? File.ReadAllText(filename) : Console.In.ReadToEnd();
        var rules = GrammarConfigLoader.LoadFromEmbeddedResource($"ToyAnalyzer.Config.{parserType.ToLower()}_rules.json");

        IParser parser = parserType.ToLower() switch
        {
            "ll1" => Parser.LL1.LL1Parser.Create(CreateLexer(source), rules),
            "lr1" => Parser.LR1.LR1Parser.Create(CreateLexer(source), rules),
            _ => throw new ArgumentException($"Invalid parser type: {parserType}")
        };

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

    private static bool TryParseArgs(string[] args, out string? filename, out string parserType)
    {
        filename = null;
        parserType = "lr1"; // 默认使用LR1分析器

        if (args.Length == 0)
        {
            return true;
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-p" || args[i] == "--parser")
            {
                if (i + 1 >= args.Length)
                {
                    Console.WriteLine("Missing parser type after -p/--parser!");
                    return false;
                }
                parserType = args[++i];
                if (parserType != "ll1" && parserType != "lr1")
                {
                    Console.WriteLine("Invalid parser type! Use 'll1' or 'lr1'");
                    return false;
                }
            }
            else
            {
                if (filename != null)
                {
                    Console.WriteLine("Too many arguments!");
                    return false;
                }
                filename = args[i];
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"File not found: {filename}");
                    return false;
                }
            }
        }

        return true;
    }

    private static Lexer.Lexer CreateLexer(string source)
    {
        var rules = LexerConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.lexer_rules.json");
        return new Lexer.Lexer(source, rules);
    }
}
