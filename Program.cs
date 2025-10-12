using System.Timers;
using FoldersSynchronizer.Core;
using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    private static System.Timers.Timer aTimer;
    private static FilesSynchronizer _filesSynchronizer;
    private const string pressAnyKeyMessage = "\nPress the Enter key to exit the application...\n";
    private const string appStartedMessage = "The application started at {0:HH:mm:ss.fff}";
    private const string syncStartedMessage = "The Files Synchronization started at {0:HH:mm:ss.fff}";

    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line bellow:
        args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt", "--repeatTimePeriod", "3000", "--logPreActions"];

        var argumentsParameters = ArgumentsProcessor.GetParametersFromArguments(args);
        _filesSynchronizer = new FilesSynchronizer(argumentsParameters);

        if (argumentsParameters.RepeatTimePeriodValue == 0)
        {
            var elapsedEventArgs = new ElapsedEventArgs(DateTime.Now);
            Console.WriteLine(syncStartedMessage, elapsedEventArgs.SignalTime);
            _filesSynchronizer.SynchronizeFiles();
        }
        else
        {
            SetTimer(argumentsParameters.RepeatTimePeriodValue);

            Console.WriteLine(pressAnyKeyMessage);
            Console.WriteLine(appStartedMessage, DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();
        }
    }

    private static void SetTimer(int periodMilliseconds)
    {
        aTimer = new System.Timers.Timer(periodMilliseconds);

        aTimer.Elapsed += StartFilesSync!;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private static void StartFilesSync(object source, ElapsedEventArgs e)
    {
        Console.WriteLine(syncStartedMessage, e.SignalTime);
        _filesSynchronizer.SynchronizeFiles();
    }
}