using System.Text.RegularExpressions;

namespace ToyAnalyzer.Lexer;

internal class LexerRule(string tokenType, string pattern)
{
    public string TokenType { get; } = tokenType;
    public Regex Pattern { get; } = new(pattern, RegexOptions.Compiled);
}
