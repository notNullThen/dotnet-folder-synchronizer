namespace FoldersSynchronizer.Support;

public class Logger
{
  private readonly string _logsFilePath;
  private readonly bool _consoleLog = true;
  public Logger(string logsFilePath, bool consoleLog = true)
  {
    try
    {
      File.Delete(logsFilePath);
    }
    catch (Exception exception)
    {
      throw new Exception($"\n🚫 Cannot delete or access log file: '{logsFilePath}'.\n💡 It's a good idea to check if the path provided is correct.\nError details are below:\n{exception.Message}");
    }
    _logsFilePath = logsFilePath;
    _consoleLog = consoleLog;
  }

  public void LogInfo(string message)
  {
    var header = "ℹ️ ";
    message = header + message;

    Log(message);
  }

  public void LogSuccess(string message)
  {
    var header = "✅ ";
    message = header + message;

    Log(message);
  }

  public void LogAlert(string message)
  {
    var alertLabel = "⚠️⚠️⚠️";
    var header = $"\n\n{alertLabel} ";
    message = header + message + $" {alertLabel}\n\n";

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