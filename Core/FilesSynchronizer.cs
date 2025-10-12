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

    public void SynchronizeFiles()
    {
      _dataReceiver.ReceiveData();

      _dirProcessor.PerformDirsScan();
      _dirProcessor.PerformDirsDeletion();
      _dirProcessor.PerformDirsCreation();

      _fileProcessor.PerformFilesScan();
      _fileProcessor.PerformFilesDeletion();
      _fileProcessor.PerformFilesCopying();
    }
  }
}