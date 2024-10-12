namespace ToyAnalyzer.Lexer;

internal class Lexer(string source, List<LexerRule> rules)
{
    private readonly string _source = source;
    private readonly List<LexerRule> _rules = rules;

    private int _position = 0;
    private int _line = 1;
    private int _column = 1;

    /// <summary>
    /// 获取下一个 Token
    /// </summary>
    /// <returns>Token</returns>
    /// <exception cref="Exception">无法识别的字符</exception>
    public Token NextToken()
    {
        if (_position >= _source.Length)
        {
            return new Token("EOF", "", _line, _column);
        }

        // 跳过空白字符
        while (_position < _source.Length && char.IsWhiteSpace(_source[_position]))
        {
            UpdatePosition(_source[_position].ToString());
            _position++;
        }

        // 匹配规则
        foreach (var rule in _rules)
        {
            var match = rule.RegexPattern.Match(_source, _position);
            if (match.Success && match.Index == _position)
            {
                var token = new Token(rule.TokenType, match.Value, _line, _column);
                UpdatePosition(match.Value);
                _position += match.Length;
                return token;
            }
        }

        throw new Exception($"Unexpected character '{_source[_position]}' at {_line}:{_column}");
    }

    private void UpdatePosition(string matchedText)
    {
        var lines = matchedText.Split('\n').Length - 1;
        if (lines > 0)
        {
            _line += lines;
            _column = matchedText.Length - matchedText.LastIndexOf('\n');
        }
        else
        {
            _column += matchedText.Length;
        }
    }
}
