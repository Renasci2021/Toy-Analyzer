namespace ToyAnalyzer.Lexer;

internal class Token(string type, string value, int line, int column)
{
    public string Type { get; } = type;
    public string Value { get; } = value;
    public int Line { get; } = line;
    public int Column { get; } = column;
}
