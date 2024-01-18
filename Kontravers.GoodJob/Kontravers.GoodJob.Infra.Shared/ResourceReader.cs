using System.Reflection;
using System.Text;

namespace Kontravers.GoodJob.Infra.Shared;

public class ResourceReader
{
    public static async Task<string[]> ReadTextLinesAsync(string fileName)
    {
        var filePath = CombineTestDataFilePath(fileName);
        var assembly = typeof(ResourceReader).GetTypeInfo().Assembly;
        await using var stream = assembly.GetManifestResourceStream(filePath);
        using var streamReader = new StreamReader(stream!, Encoding.UTF8);
        var content = await streamReader.ReadToEndAsync();

        var lines = content
            .Split(Environment.NewLine)
            .Select(l => l.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

        return lines;
    }

    private static string CombineTestDataFilePath(string fileName)
    {
        var assembly = typeof(ResourceReader).GetTypeInfo().Assembly;
        var assemblyName = assembly.GetName().Name;
        return $"{assemblyName}.EmailTemplates.{fileName}";
    }
}