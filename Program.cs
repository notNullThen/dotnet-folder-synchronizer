using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt"];
        // For debugging in VS Code uncomment the line bellow:

        var argumentParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(argumentParameters.LogsFilePath);

        var dirReceiver = new DirReceiver(argumentParameters);
        var folderScanner = new DirScanner(argumentParameters);

        dirReceiver.ReceiveData();

        folderScanner.PerformFoldersScan();

        DirScanner.PerformFoldersDeletion();
    }
}