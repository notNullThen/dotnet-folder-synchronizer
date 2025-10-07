using System.Text.Json;

namespace FoldersSynchronizer;

class Program
{
    private static readonly JsonSerializerOptions JsonPretty = new() { WriteIndented = true };

    static void Main(string[] args)
    {
        var parameters = ArgumentsHelper.GetParameters(args);

        var dirFiles = Support.GetFilesFromDir(parameters.SourceDirPath);

        Console.WriteLine(JsonSerializer.Serialize(dirFiles, JsonPretty));
    }
}