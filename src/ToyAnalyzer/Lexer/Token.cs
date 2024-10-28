namespace ToyAnalyzer.Lexer;

internal record Token(string Type, string Value, int Line, int Column);

// Token 类型，越靠前值越小，优先级越高
internal enum TokenCategory
{
    Keyword,
    Identifier,
    Literal,
    Operator,
    Delimiter,
}
