using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt"];

        var argumentParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var logger = new Logger(argumentParameters.LogsFilePath);
        var folderSynchronizerCore = new FolderSynchronizerCore(logger, argumentParameters);

        folderSynchronizerCore.RecieveFiles();
        folderSynchronizerCore.ScanDir();
        if (argumentParameters.DebugValue) folderSynchronizerCore.Debug();
        folderSynchronizerCore.DeleteTargetDirs();
        folderSynchronizerCore.DeleteTargetFiles();
        folderSynchronizerCore.CopyFiles();
    }
}