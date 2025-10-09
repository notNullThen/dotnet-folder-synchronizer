using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        args = ["-sourceDir", "./SourceFolder", "-targetDir", "./TargetFolder", "-logs", "./logs.txt", "-debug"];

        var parameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(parameters.LogsFilePath);
        var filesReceiver = new FilesReceiver(logger, parameters);

        filesReceiver.RecieveFiles();
        filesReceiver.ScanDir();
        filesReceiver.ExecuteTasks();
    }
}