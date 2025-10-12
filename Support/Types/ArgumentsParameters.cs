namespace FoldersSynchronizer.Support.Types;

public class ArgumentsParameters
{
  public const string Prefix = "--";
  public static readonly string SourceDirArgument = Prefix + "sourceDir";
  public static readonly string TargetDirArgument = Prefix + "targetDir";
  public static readonly string LogsArgument = Prefix + "logs";
  public static readonly string RepeatTimePeriod = Prefix + "repeatTimePeriod";
  public static readonly string LogPreActions = Prefix + "logPreActions";
  public string SourceDirPath { get; set; }
  public string TargetDirPath { get; set; }
  public string LogsFilePath { get; set; }
  public bool LogPreActionsValue { get; set; }
  public int RepeatTimePeriodValue { get; set; }
}