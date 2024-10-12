namespace ToyAnalyzer.Lexer;

internal record Token(string Type, string Value, int Line, int Column);
