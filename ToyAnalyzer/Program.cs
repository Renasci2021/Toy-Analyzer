using ToyAnalyzer.Lexer;

namespace ToyAnalyzer;

class Program
{
    static void Main(string[] args)
    {
        var source = "let x = 10;";

        var rules = new List<LexerRule>
        {
            new("LET", "let"),
            new("IDENTIFIER", "[a-zA-Z_][a-zA-Z0-9_]*"),
            new("ASSIGN", "="),
            new("NUMBER", "[0-9]+"),
            new("SEMICOLON", ";"),
        };

        var lexer = new Lexer.Lexer(source, rules);
        Token token;
        while (true)
        {
            token = lexer.NextToken();

            if (token.Type == "EOF")
            {
                break;
            }

            Console.WriteLine($"Token: {token.Type}, Value: {token.Value}, Line: {token.Line}, Column: {token.Column}");
        }
    }
}
