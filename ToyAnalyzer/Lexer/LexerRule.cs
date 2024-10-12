using System.Text.RegularExpressions;

namespace ToyAnalyzer.Lexer;

internal record LexerRule(string TokenType, string Pattern)
{
    public Regex RegexPattern { get; } = new Regex(Pattern, RegexOptions.Compiled);
}
