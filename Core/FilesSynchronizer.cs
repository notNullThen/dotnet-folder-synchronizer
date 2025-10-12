using FoldersSynchronizer.Support;
using FoldersSynchronizer.Support.Types;

namespace FoldersSynchronizer.Core
{
  public class FilesSynchronizer
  {
    private readonly Logger _logger;
    private readonly DataReceiver _dataReceiver;
    private readonly DirProcessor _dirProcessor;
    private readonly FileProcessor _fileProcessor;

    public FilesSynchronizer(ArgumentsParameters argumentsParameters)
    {
      _logger = new(argumentsParameters.LogsFilePath);
      _dataReceiver = new(argumentsParameters, _logger);
      _dirProcessor = new(argumentsParameters, _logger);
      _fileProcessor = new(argumentsParameters, _logger);
    }

    public void RunFileSync()
    {
      var startTime = DateTime.Now;
      _logger.Log($@"
===============================================
🔄🔄🔄 FILES SYNCHRONIZATION STARTED... 🔄🔄🔄
===============================================
Started at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

      _dataReceiver.EraseData();
      _dataReceiver.ReceiveData();

      _dirProcessor.PerformDirsScan();
      _dirProcessor.PerformDirsDeletion();
      _dirProcessor.PerformDirsCreation();

      _fileProcessor.PerformFilesScan();
      _fileProcessor.PerformFilesDeletion();
      _fileProcessor.PerformFilesCopying();

      _logger.Log(@"
===============================================
🎉🎉🎉 FILES SYNCHRONIZATION COMPLETED! 🎉🎉🎉
===============================================
Completed at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
  }
}