namespace FoldersSynchronizer.Support;

public class Logger
{
  private string _logsFilePath;
  private bool _consoleLog = true;
  public Logger(string logsFilePath, bool consoleLog = true)
  {
    File.Delete(logsFilePath);
    _logsFilePath = logsFilePath;
  }

  public void LogInfo(string message)
  {
    var header = "ℹ️ Info:\n";
    message = header + message;

    if (_consoleLog)
      Console.Write(message);
    File.AppendAllText(_logsFilePath, message);
  }

  public void LogSuccess(string message)
  {
    var header = "✅ Success:\n";
    message = header + message;

    if (_consoleLog)
      Console.Write(message);
    File.AppendAllText(_logsFilePath, message);
  }

  public void LogError(string message, bool throwException = false)
  {
    var header = "💥💥💥 Error 💥💥💥\n";
    message = header + message;

    if (_consoleLog)
      Console.Write(message);
    File.AppendAllText(_logsFilePath, message);

    if (throwException) throw new Exception(message);
  }
}