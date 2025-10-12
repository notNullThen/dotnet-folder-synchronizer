using FoldersSynchronizer.Core;
using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        // args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt", "--logPreActions"];

        var argumentsParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        var filesSynchronizer = new FilesSynchronizer(argumentsParameters);

        filesSynchronizer.SynchronizeFiles();
    }
}