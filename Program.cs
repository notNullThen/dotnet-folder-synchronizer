using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt", "--logPreActions"];
        // For debugging in VS Code uncomment the line bellow:

        var argumentsParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(argumentsParameters.LogsFilePath);

        var dataReceiver = new DataReceiver(argumentsParameters, logger);
        var dirProcessor = new DirProcessor(argumentsParameters, logger);
        var fileProcessor = new FileProcessor(argumentsParameters, logger);

        dataReceiver.ReceiveData();

        dirProcessor.PerformDirsScan();
        fileProcessor.PerformFilesScan();

        dirProcessor.PerformDirsDeletion();
        fileProcessor.PerformFilesDeletion();
        fileProcessor.PerformFilesCopying();
    }
}