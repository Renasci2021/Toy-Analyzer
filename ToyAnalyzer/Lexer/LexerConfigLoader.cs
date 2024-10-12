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
        return JsonSerializer.Deserialize<List<LexerRule>>(jsonContent) ?? throw new Exception("Failed to deserialize JSON");
    }
}
