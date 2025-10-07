using System.Text.Json;

namespace FoldersSynchronizer;

class Program
{
    private static readonly JsonSerializerOptions JsonPretty = new() { WriteIndented = true };

    static void Main(string[] args)
    {
        // debug
        // args = ["-sourceDir", "./SourceFolder", "-targetDir", "./NewFolder"];

        var parameters = ArgumentsHelper.GetParameters(args);

        var sourceDirDetails = Support.GetFilesFromDir(parameters.SourceDirPath);

        Support.DirDetails targetDirDetails;

        if (Directory.Exists(parameters.TargetDirPath))
            targetDirDetails = Support.GetFilesFromDir(parameters.TargetDirPath);
        else
            Directory.CreateDirectory(parameters.TargetDirPath);

        Console.WriteLine(@$"Source Dir details:
{JsonSerializer.Serialize(sourceDirDetails, JsonPretty)}

Target Dir Details:
");
    }
}