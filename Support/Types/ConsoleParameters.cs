namespace FoldersSynchronizer.Support;

public class ConsoleParameters
{
  public const string SourceDirArgument = "-sourceDir";
  public const string TargetDirArgument = "-targetDir";
  public const string DebugArgument = "-debug";
  public string SourceDirPath { get; set; }
  public string TargetDirPath { get; set; }
  public bool DebugValue { get; set; }
}