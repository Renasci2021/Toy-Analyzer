using System.Text.RegularExpressions;

namespace ToyAnalyzer.Lexer;

internal record LexerRule
{
    public string TokenType { get; }
    public Regex Pattern { get; }
    public TokenCategory TokenCategory { get; }

    public LexerRule(string tokenType, string pattern, string tokenCategory)
    {
        TokenType = tokenType;
        Pattern = new Regex(pattern, RegexOptions.Compiled);
        TokenCategory = Enum.Parse<TokenCategory>(tokenCategory);
    }
}
