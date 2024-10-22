using System.Xml.Linq;
using ToyAnalyzer.Lexer;

namespace ToyAnalyzer.Parser;

internal class Parser(Lexer.Lexer lexer, Dictionary<(string, string), List<string>> parseTable)
{
    private readonly Lexer.Lexer _lexer = lexer;
    private readonly Dictionary<(string, string), List<string>> _parseTable = parseTable;

    public XElement Parse()
    {
        XElement root = new("PARSER");

        Stack<(string, XElement)> stack = [];
        stack.Push(("PROGRAM", root));

        Token currentToken = _lexer.NextToken();

        while (stack.Count > 0)
        {
            var (top, parentElement) = stack.Pop();

            if (IsTerminal(top))
            {
                if (top == currentToken.Type)
                {
                    var matchedElement = new XElement(top, currentToken.Value);

                    parentElement.Add(matchedElement);

                    currentToken = _lexer.NextToken();
                }
                else
                {
                    throw new Exception($"Syntax error: expected {top}, found {currentToken.Type}");
                }
            }
            else
            {
                if (_parseTable.TryGetValue((top, currentToken.Type), out var production))
                {
                    // Skip empty productions
                    if (production[0] == "empty")
                    {
                        continue;
                    }

                    // Adjust xml tree for productions that end with REMAINING
                    if (top.EndsWith("REMAINING"))
                    {
                        production.AsEnumerable().Reverse().ToList().ForEach(symbol => stack.Push((symbol, parentElement)));
                        continue;
                    }

                    var nonTerminalElement = new XElement(top);
                    parentElement.Add(nonTerminalElement);

                    production.AsEnumerable().Reverse().ToList().ForEach(symbol => stack.Push((symbol, nonTerminalElement)));
                }
                else
                {
                    throw new Exception($"Syntax error: no production for {top} -> {currentToken.Type}");
                }
            }
        }

        return root;
    }

    private static bool IsTerminal(string symbol)
    {
        return char.IsLower(symbol[0]);
    }
}
