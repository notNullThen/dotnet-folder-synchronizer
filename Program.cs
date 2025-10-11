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

        var dataReceiver = new DataReceiver(argumentParameters);
        var folderScanner = new DirProcessor(argumentParameters);

        dataReceiver.ReceiveData();

        folderScanner.PerformFoldersScan();

        DirProcessor.PerformFoldersDeletion();
    }
}