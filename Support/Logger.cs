namespace FoldersSynchronizer.Support;

public class Logger
{
  private readonly string _logsFilePath;
  private readonly bool _consoleLog = true;
  public Logger(string logsFilePath, bool consoleLog = true)
  {
    File.Delete(logsFilePath);
    _logsFilePath = logsFilePath;
    _consoleLog = consoleLog;
  }

  public void LogInfo(string message)
  {
    var header = "ℹ️ Info:\n";
    message = header + message;

    Log(message);
  }

  public void LogSuccess(string message)
  {
    var header = "✅ Success:\n";
    message = header + message;

    Log(message);
  }

  public void LogAlert(string message)
  {
    var header = "⚠️ Alert\n";
    message = header + message;

    Log(message);
  }

  public void LogError(string message, bool throwException = false)
  {
    var header = "💥💥💥 Error 💥💥💥\n";
    message = header + message;

    Log(message, throwException);
  }

  public void Log(string message, bool throwException = false)
  {
    message += "\n\n";

    if (_consoleLog)
      Console.Write(message);
    File.AppendAllText(_logsFilePath, message);

    if (throwException) throw new Exception(message);
  }
}