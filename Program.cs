using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt"];
        // For debugging in VS Code uncomment the line bellow:

        var argumentsParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(argumentsParameters.LogsFilePath);

        var dataReceiver = new DataReceiver(argumentsParameters);
        var dirProcessor = new DirProcessor(argumentsParameters);

        dataReceiver.ReceiveData();

        dirProcessor.PerformFoldersScan();

        DirProcessor.PerformFoldersDeletion();
    }
}