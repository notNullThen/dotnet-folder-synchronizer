namespace FoldersSynchronizer.Support;

using FoldersSynchronizer.Support.Details;

public class Logger
{
  private static string LoggerFilePath = Path.Combine([".", "logs.txt"]);

  public void LogFileProcessed(FileDetails fileDetails, bool changed)
  {
    var message = string.Empty;

    if (changed)
      message = $"\nThe target file \"{fileDetails.Name}\" was changed.\nPath: {fileDetails.Path}\n";
    else
      message = $"\nThe target file\"{fileDetails.Name}\" is identical with source, therefore was NOT changed.\nPath: {fileDetails.Path}\n";

    File.AppendAllText(LoggerFilePath, message);
  }
}