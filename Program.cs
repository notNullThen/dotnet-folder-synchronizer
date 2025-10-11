using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt"];
        // For debugging in VS Code uncomment the line bellow:

        var argumentsProcessor = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(argumentsProcessor.LogsFilePath);

        var dataReceiver = new DataReceiver(argumentsProcessor);
        var dirProcessor = new DirProcessor(argumentsProcessor);

        dataReceiver.ReceiveData();

        dirProcessor.PerformFoldersScan();

        DirProcessor.PerformFoldersDeletion();
    }
}