using ToyAnalyzer.Lexer;

namespace ToyAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        var source = "let x = 10;";

        var rules = LexerConfigLoader.LoadFromEmbeddedResource("ToyAnalyzer.Config.lexer_rules.json");
        var lexer = new Lexer.Lexer(source, rules);

        while (true)
        {
            var token = lexer.NextToken();

            if (token.Type == "EOF")
            {
                break;
            }

            Console.WriteLine($"Token: {token.Type}, Value: {token.Value}, Line: {token.Line}, Column: {token.Column}");
        }
    }
}
