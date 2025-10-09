namespace FoldersSynchronizer.Support;

public class ArgumentsParameters
{
  public const string Prefix = "--";
  public static readonly string SourceDirArgument = Prefix + "sourceDir";
  public static readonly string TargetDirArgument = Prefix + "targetDir";
  public static readonly string DebugArgument = Prefix + "debug";
  public static readonly string LogsArgument = Prefix + "logs";
  public string SourceDirPath { get; set; }
  public string TargetDirPath { get; set; }
  public string LogsFilePath { get; set; }
  public bool DebugValue { get; set; }
}