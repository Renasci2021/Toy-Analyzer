using ToyAnalyzer.Parser.Common;

namespace ToyAnalyzer.Parser.LL1;

internal static class LL1TableGenerator
{
    public static Dictionary<(string, string), List<string>> GenerateTable(List<GrammarRule> rules)
    {
        var firstSets = CalculateFirstSets(rules);
        var followSets = CalculateFollowSets(rules, firstSets);
        var table = new Dictionary<(string, string), List<string>>();

        foreach (var rule in rules)
        {
            string left = rule.Left;
            foreach (var production in rule.Right)
            {
                var firstSet = First(production, firstSets);

                foreach (var terminal in firstSet)
                {
                    if (terminal != "empty")
                    {
                        table[(left, terminal)] = production;
                    }
                }

                // If empty is in the first set, add follow set of the non-terminal
                if (firstSet.Contains("empty"))
                {
                    foreach (var followSymbol in followSets[left])
                    {
                        table[(left, followSymbol)] = production;
                    }
                }
            }
        }

        return table;
    }

    private static Dictionary<string, HashSet<string>> CalculateFirstSets(List<GrammarRule> rules)
    {
        var firstSets = new Dictionary<string, HashSet<string>>();

        foreach (var rule in rules)
        {
            firstSets[rule.Left] = [];
        }

        bool changed;
        do
        {
            changed = false;

            foreach (var rule in rules)
            {
                foreach (var production in rule.Right)
                {
                    var firstSet = First(production, firstSets);
                    int initialCount = firstSets[rule.Left].Count;
                    firstSets[rule.Left].UnionWith(firstSet);

                    // If any new elements were added to the First set, set changed to true
                    if (firstSets[rule.Left].Count > initialCount)
                    {
                        changed = true;
                    }
                }
            }
        } while (changed);

        return firstSets;
    }

    private static HashSet<string> First(List<string> symbols, Dictionary<string, HashSet<string>> firstSets)
    {
        var result = new HashSet<string>();

        foreach (var symbol in symbols)
        {
            // If symbol is a terminal or empty, add it to the result and stop
            if (char.IsLower(symbol[0]) || symbol == "empty")
            {
                result.Add(symbol);
                break;
            }

            // Otherwise, add all non-empty symbols from the first set of the non-terminal
            if (firstSets.TryGetValue(symbol, out HashSet<string>? value))
            {
                result.UnionWith(value.Where(s => s != "empty"));

                // If empty is not in the first set of the current symbol, stop
                if (!value.Contains("empty"))
                {
                    break;
                }
            }
        }

        // If all symbols can derive empty, add empty to the result
        if (symbols.All(s => firstSets.ContainsKey(s) && firstSets[s].Contains("empty")))
        {
            result.Add("empty");
        }

        return result;
    }

    private static Dictionary<string, HashSet<string>> CalculateFollowSets(
        List<GrammarRule> rules, Dictionary<string, HashSet<string>> firstSets)
    {
        var followSets = new Dictionary<string, HashSet<string>>
        {
            // Add $ (end of input marker) to the follow set of the start symbol
            {"PROGRAM", ["$"] }
        };

        bool changed;
        do
        {
            changed = false;

            foreach (var rule in rules)
            {
                string left = rule.Left;

                foreach (var production in rule.Right)
                {
                    for (int i = 0; i < production.Count; i++)
                    {
                        var symbol = production[i];

                        if (!char.IsLower(symbol[0]))
                        {
                            var followSet = new HashSet<string>();

                            // Get first set of the remaining symbols after the current symbol
                            var suffix = production.Skip(i + 1).ToList();
                            var firstSet = First(suffix, firstSets);

                            followSet.UnionWith(firstSet.Where(s => s != "empty"));

                            // If empty is in the first set of the suffix, add follow of the left-hand side
                            if (firstSet.Contains("empty") || suffix.Count == 0)
                            {
                                followSet.UnionWith(followSets[left]);
                            }

                            if (!followSets.TryGetValue(symbol, out HashSet<string>? value))
                            {
                                value = ([]);
                                followSets[symbol] = value;
                            }

                            int initialCount = value.Count;
                            value.UnionWith(followSet);

                            if (value.Count > initialCount)
                            {
                                changed = true;
                            }
                        }
                    }
                }
            }
        } while (changed);

        return followSets;
    }
}
