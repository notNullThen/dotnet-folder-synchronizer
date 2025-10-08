using System.Text.Json;

namespace FoldersSynchronizer;

class Program
{
    private static readonly JsonSerializerOptions JsonPretty = new() { WriteIndented = true };

    static void Main(string[] args)
    {
        // debug
        // args = ["-sourceDir", "./SourceFolder", "-targetDir", "./NewFolder"];

        var parameters = ArgumentsHelper.GetConsoleParameters(args);

        var sourceDirDetails = Support.GetDirDetails(parameters.SourceDirPath);

        Support.DirDetails targetDirDetails;

        if (Directory.Exists(parameters.TargetDirPath))
            targetDirDetails = Support.GetDirDetails(parameters.TargetDirPath);
        else
            Directory.CreateDirectory(parameters.TargetDirPath);

        foreach (var arg in args)
        {
            if (arg.Contains("-debug"))
            {
                Console.WriteLine(@$"Source Dir details:
{JsonSerializer.Serialize(sourceDirDetails, JsonPretty)}

Target Dir Details:
");
            }
        }
    }

    static void Debug()
    {

    }
}