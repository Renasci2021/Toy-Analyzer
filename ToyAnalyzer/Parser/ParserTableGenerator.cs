namespace ToyAnalyzer.Parser;

internal class ParserTableGenerator
{
    public static Dictionary<(string, string), List<string>> Generate(Dictionary<string, GrammarRule> rules)
    {
        var table = new Dictionary<(string, string), List<string>>();

        foreach (var rule in rules.Values)
        {
            foreach (var production in rule.Right)
            {
                var first = ComputeFirstSet(rules, production);
                foreach (var symbol in first)
                {
                    if (symbol == "empty")
                    {
                        foreach (var follow in rule.Follow)
                        {
                            AddToTable(table, rule.Left, follow, production);
                        }
                    }
                    else
                    {
                        AddToTable(table, rule.Left, symbol, production);
                    }
                }
            }
        }

        PrintTable(table);
        return table;
    }

    private static void PrintTable(Dictionary<(string, string), List<string>> table)
    {
        foreach (var ((left, symbol), production) in table)
        {
            Console.WriteLine($"{left} -> {symbol}: {string.Join(" ", production)}");
        }
    }

    private static HashSet<string> ComputeFirstSet(Dictionary<string, GrammarRule> rules, List<string> production)
    {
        var first = new HashSet<string>();
        var containsEmpty = true;

        foreach (var symbol in production)
        {
            if (rules.ContainsKey(symbol)) // symbol 是非终结符
            {
                first.UnionWith(rules[symbol].First);
                if (!rules[symbol].First.Contains("empty"))
                {
                    containsEmpty = false;
                    break;
                }
            }
            else // symbol 是终结符
            {
                first.Add(symbol);
                containsEmpty = false;
                break;
            }
        }

        // 如果所有符号都可以推导出 empty，则需要将 "empty" 加入 FIRST 集合
        if (containsEmpty)
        {
            first.Add("empty");
        }

        return first;
    }

    private static void AddToTable(Dictionary<(string, string), List<string>> table, string left, string symbol, List<string> production)
    {
        if (!table.ContainsKey((left, symbol)))
        {
            table[(left, symbol)] = production;
        }
        else
        {
            throw new Exception($"Conflict in table: {left} -> {symbol}");
        }
    }
}
