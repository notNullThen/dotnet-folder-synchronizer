
using System.Text.Json;
using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    private static readonly FilesReceiver _fileReceiver = new();

    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        // args = ["-debug", "-sourceDir", "./SourceFolder", "-targetDir", "./TargetFolder"];

        var parameters = ArgumentsProcessor.GetConsoleParameters(args);

        _fileReceiver.RecieveFiles(parameters);
    }
}