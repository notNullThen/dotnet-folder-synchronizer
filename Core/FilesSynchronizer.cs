using FoldersSynchronizer.Support;
using FoldersSynchronizer.Support.Types;

namespace FoldersSynchronizer.Core
{
  public class FilesSynchronizer
  {
    private readonly ArgumentsParameters _argumentsParameters;
    private readonly Logger _logger;
    private readonly DataReceiver _dataReceiver;
    private readonly DirProcessor _dirProcessor;
    private readonly FileProcessor _fileProcessor;

    public FilesSynchronizer(ArgumentsParameters argumentsParameters)
    {
      _logger = new(argumentsParameters.LogsFilePathValue);
      _dataReceiver = new(argumentsParameters, _logger);
      _dirProcessor = new(argumentsParameters, _logger);
      _fileProcessor = new(argumentsParameters, _logger);
      _argumentsParameters = argumentsParameters;
    }

    public void RunFileSync()
    {
      var startTime = DateTime.Now;
      var parsedRepeatTimePeriod = Utils.ParseMillisecondsToTimeString(_argumentsParameters.RepeatTimePeriodValue);

      _logger.Log($@"===============================================
🔄🔄🔄 FILES SYNCHRONIZATION STARTED... 🔄🔄🔄
===============================================
Started at: {startTime:yyyy-MM-dd HH:mm:ss} for each {parsedRepeatTimePeriod}
===============================================
To stop synchronization press Ctrl + C");

      _dataReceiver.EraseData();
      _dataReceiver.ReceiveData();

      _dirProcessor.PerformDirsScan();
      _dirProcessor.PerformDirsDeletion();
      _dirProcessor.PerformDirsCreation();

      _fileProcessor.PerformFilesScan();
      _fileProcessor.PerformFilesDeletion();
      _fileProcessor.PerformFilesCopying();

      var endTime = DateTime.Now;
      var tookTime = endTime - startTime;

      _logger.Log($@"
===============================================
🎉🎉🎉 FILES SYNCHRONIZATION COMPLETED! 🎉🎉🎉
===============================================
Completed at: {endTime:yyyy-MM-dd HH:mm:ss}
Took time: {tookTime:hh\:mm\:ss}
Next run in: {parsedRepeatTimePeriod}
===============================================
To stop synchronization press Ctrl + C");
    }
  }
}