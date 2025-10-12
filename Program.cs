using System.Timers;
using FoldersSynchronizer.Core;
using FoldersSynchronizer.Support;

namespace FoldersSynchronizer;

class Program
{
    private static System.Timers.Timer aTimer;
    private static FilesSynchronizer _filesSynchronizer;

    static void Main(string[] args)
    {
        // For debugging in VS Code uncomment the line below:
        // args = ["--sourceDir", "../../../DataToTestOn/SourceFolder", "--targetDir", "../../../DataToTestOn/TargetFolder", "--logs", "../../../DataToTestOn/logs.txt", "--repeatTimePeriod", "3000", "--logPreActions"];

        var parameters = ArgumentsProcessor.GetParametersFromArguments(args);
        _filesSynchronizer = new FilesSynchronizer(parameters);

        if (parameters.RepeatTimePeriodValue == 0)
        {
            _filesSynchronizer.RunFileSync();
        }
        else
        {
            _filesSynchronizer.RunFileSync();
            SetTimer(parameters.RepeatTimePeriodValue);

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
        _filesSynchronizer.RunFileSync();
    }
}