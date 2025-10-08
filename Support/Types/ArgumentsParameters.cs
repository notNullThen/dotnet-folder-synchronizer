namespace FoldersSynchronizer.Support;

public class ArgumentsParameters
{
  public const string SourceDirArgument = "-sourceDir";
  public const string TargetDirArgument = "-targetDir";
  public const string DebugArgument = "-debug";
  public const string LogsArgument = "-logs";
  public string SourceDirPath { get; set; }
  public string TargetDirPath { get; set; }
  public bool DebugValue { get; set; }
  public string LogsValue { get; set; }
}