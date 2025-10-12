using System.Timers;
using FoldersSynchronizer.Core;
using FoldersSynchronizer.Support;

namespace FoldersSynchronizer
{
	class Program
	{
		private static System.Timers.Timer aTimer;
		private static Logger _logger;
		private static FilesSynchronizer _filesSynchronizer;
		private static bool _isRunning = false; // Prevent overlapping runs

		static void Main(string[] args)
		{
			// For debugging in VS Code uncomment the line below:
			// args = [
			// 	"--sourceDir", "../../../DataToTestOn/SourceFolder",
			// 	"--targetDir",
			// 	"../../../DataToTestOn/TargetFolder",
			// 	"--logs",
			// 	"../../../DataToTestOn/logs.txt",
			// 	"--repeatTimePeriod",
			// 	"3000",
			// 	"--logPreActions"];

			var argumentParameters = ArgumentsProcessor.GetParametersFromArguments(args);
			_logger = new(argumentParameters.LogsFilePathValue);
			_filesSynchronizer = new FilesSynchronizer(argumentParameters);

			if (argumentParameters.RepeatTimePeriodValue == 0)
			{
				_filesSynchronizer.RunFileSync();
			}
			else
			{
				_filesSynchronizer.RunFileSync();
				SetTimer(argumentParameters.RepeatTimePeriodValue);

				Console.ReadLine();

				aTimer.Stop();
				aTimer.Dispose();
			}
		}

		private static void SetTimer(int periodMilliseconds)
		{
			aTimer = new System.Timers.Timer(periodMilliseconds)
			{
				AutoReset = true,
				Enabled = true
			};

			aTimer.Elapsed += StartFilesSync!;
		}

		private static void StartFilesSync(object source, ElapsedEventArgs e)
		{
			if (_isRunning)
			{
				_logger.LogAlert("Previous Files Synchronization is not finished yet. Skipping this cycle...");
				return;
			}

			try
			{
				_isRunning = true;
				_filesSynchronizer.RunFileSync();
			}
			finally
			{
				_isRunning = false;
			}
		}
	}
}
