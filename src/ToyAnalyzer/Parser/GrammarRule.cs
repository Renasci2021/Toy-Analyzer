namespace ToyAnalyzer.Parser;

internal record GrammarRule
{
    public string Left { get; }
    public List<List<string>> Right { get; }

    public GrammarRule(string left, List<List<string>> right)
    {
        Left = left;
        Right = right;
    }
}
