using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    private static readonly FilesReceiver _fileReceiver = new();

    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        // args = ["-sourceDir", "./SourceFolder", "-targetDir", "./TargetFolder", "-logs", "./logs.txt", "-debug"];

        var parameters = ArgumentsProcessor.GetParametersFromArguments(args);

        _fileReceiver.RecieveFiles(parameters);
    }
}