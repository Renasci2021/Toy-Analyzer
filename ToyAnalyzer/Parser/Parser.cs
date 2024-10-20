using ToyAnalyzer.Lexer;

namespace ToyAnalyzer.Parser;

internal class Parser(Lexer.Lexer lexer, Dictionary<string, GrammarRule> rules, Dictionary<(string, string), List<string>> parseTable)
{
    private readonly Dictionary<string, GrammarRule> _rules = rules;
    private readonly Lexer.Lexer _lexer = lexer;
    private readonly Dictionary<(string, string), List<string>> _parseTable = parseTable;

    public void Parse()
    {
        Stack<string> _stack = new(["PROGRAM"]);
        Token currentToken = _lexer.NextToken();

        while (_stack.Count > 0)
        {
            var top = _stack.Pop();

            // 如果 top 是非终结符，查找对应的产生式
            if (_rules.ContainsKey(top))
            {
                if (_parseTable.TryGetValue((top, currentToken.Type), out var production))
                {
                    if (production.Count == 1 && production[0] == "empty") continue;

                    for (int i = production.Count - 1; i >= 0; i--)
                    {
                        _stack.Push(production[i]);
                    }
                }
                else
                {
                    Console.WriteLine($"Syntax error: {currentToken.Type}: {currentToken.Value}");
                    break;
                }
            }
            // 如果 top 是终结符，匹配当前 token
            else if (top == currentToken.Type)
            {
                Console.WriteLine($"Matched: {currentToken.Type}: {currentToken.Value}");
                currentToken = _lexer.NextToken();
            }
            // 如果终结符和当前 token 不匹配，报错
            else
            {
                Console.WriteLine($"Syntax error: {currentToken.Type}: {currentToken.Value}");
                break;
            }
        }

        if (_stack.Count > 0 || currentToken.Type != "EOF")
        {
            Console.WriteLine("Parsing failed");
        }
        else
        {
            Console.WriteLine("Syntax analysis completed");
        }
    }
}
