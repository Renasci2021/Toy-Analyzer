using System.Reflection;
using System.Text.Json;

namespace ToyAnalyzer.Lexer;

internal class LexerConfigLoader
{
    public static List<LexerRule> LoadFromEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource '{resourceName}' not found");

        using var reader = new StreamReader(stream);
        var jsonContent = reader.ReadToEnd();

        using var jsonDocument = JsonDocument.Parse(jsonContent);
        var LexerRules = new List<LexerRule>();

        foreach (var element in jsonDocument.RootElement.EnumerateArray())
        {
            var tokenType = element.GetProperty("TokenType").GetString();
            var regexPattern = element.GetProperty("Pattern").GetString();
            var tokenCategory = element.GetProperty("TokenCategory").GetString();

            LexerRules.Add(new LexerRule(tokenType!, regexPattern!, tokenCategory!));
        }

        return LexerRules;
    }
}
