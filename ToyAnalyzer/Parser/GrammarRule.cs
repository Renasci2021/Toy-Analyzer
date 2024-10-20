namespace ToyAnalyzer.Parser;

internal class GrammarRule(string left, List<List<string>> right)
{
    public string Left { get; private set; } = left;
    public List<List<string>> Right { get; private set; } = right;

    public HashSet<string> First { get; private set; } = [];
    public HashSet<string> Follow { get; private set; } = [];

    public void ComputeFirstSet(Dictionary<string, GrammarRule> grammar)
    {
        if (First.Count > 0)
        {
            return;
        }

        foreach (var production in Right)
        {
            var i = 0;
            while (i < production.Count)
            {
                var symbol = production[i];
                if (grammar.ContainsKey(symbol))
                {
                    grammar[symbol].ComputeFirstSet(grammar);
                    First.UnionWith(grammar[symbol].First);
                    if (!grammar[symbol].First.Contains("empty"))
                    {
                        break;
                    }
                }
                else
                {
                    First.Add(symbol);
                    break;
                }
                i++;
            }
        }
    }

    public void ComputeFollowSet(Dictionary<string, GrammarRule> grammar, string startSymbol)
    {
        if (Follow.Count > 0)
        {
            return;
        }

        if (Left == startSymbol)
        {
            Follow.Add("EOF");
        }

        foreach (var rule in grammar.Values)
        {
            foreach (var production in rule.Right)
            {
                var i = 0;
                while (i < production.Count)
                {
                    if (production[i] == Left)
                    {
                        if (i == production.Count - 1)
                        {
                            if (rule.Left != Left)
                            {
                                rule.ComputeFollowSet(grammar, startSymbol);
                                Follow.UnionWith(rule.Follow);
                            }
                        }
                        else
                        {
                            var j = i + 1;
                            while (j < production.Count)
                            {
                                if (grammar.ContainsKey(production[j]))
                                {
                                    Follow.UnionWith(grammar[production[j]].First);
                                    if (!grammar[production[j]].First.Contains("empty"))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    Follow.Add(production[j]);
                                    break;
                                }
                                j++;
                            }

                            if (j == production.Count)
                            {
                                rule.ComputeFollowSet(grammar, startSymbol);
                                Follow.UnionWith(rule.Follow);
                            }
                        }
                    }
                    i++;
                }
            }
        }
    }
}
