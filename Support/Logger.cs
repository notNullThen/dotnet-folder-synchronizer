namespace FoldersSynchronizer.Support;

using FoldersSynchronizer.Support.Details;

public class Logger
{
  private static string _logsFilePath;
  public Logger(string logsFilePath)
  {
    File.Delete(logsFilePath);
    _logsFilePath = logsFilePath;
  }
  public void Log(FileDetails fileDetails, bool ignored, bool consoleLog)
  {
    string message;

    if (ignored)
      message = $"The target file \"{fileDetails.Name}\" will NOT be changed as it is identical with the source.\nPath: {fileDetails.Path}\n\n";
    else
      message = $"The target file \"{fileDetails.Name}\" was changed.\nPath: {fileDetails.Path}\n\n";

    if (consoleLog)
      Console.Write(message);

    File.AppendAllText(_logsFilePath, message);
  }
}