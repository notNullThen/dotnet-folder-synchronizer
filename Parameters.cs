namespace FoldersSynchronizer;

public static class ArgumentsHelper
{
  private const string ArgsStrtuctureErrorMessage = "Arguments structure is wrong";


  public static Parameters GetParameters(string[] args)
  {
    var parsedArgs = ParseArgs(args);

    var sourceDirPath = string.Empty;
    var targetDirPath = string.Empty;

    foreach (var arg in parsedArgs)
    {
      switch (arg.Key)
      {
        case Arguments.SourceDir:
          sourceDirPath = arg.Value;
          continue;

        case Arguments.TargetDir:
          targetDirPath = arg.Value;
          continue;

        default: throw new Exception($"The entered '{arg.Key}' argument is wrong.");
      }
    }

    if (string.IsNullOrWhiteSpace(sourceDirPath))
    {
      throw new Exception($"You did not provided a '{Arguments.SourceDir}' argument");
    }

    if (string.IsNullOrWhiteSpace(targetDirPath))
    {
      throw new Exception($"You did not provided a '{Arguments.TargetDir}' argument");
    }

    return new Parameters()
    {
      SourceDirPath = sourceDirPath,
      TargetDirPath = targetDirPath
    };
  }

  private static Dictionary<string, string> ParseArgs(string[] args)
  {
    var parsedArgs = new Dictionary<string, string>();

    for (var i = 0; i < args.Count(); i++)
    {
      var arg = args[i];

      if (arg.Contains('-'))
      {
        var key = arg;
        var value = args[i + 1];

        if (value.StartsWith('-'))
        {
          throw new Exception($"{ArgsStrtuctureErrorMessage}: value of '{key}' argument starts with '-'");
        }

        parsedArgs.Add(arg, args[i + 1]);

        i++;
      }
      else if (i == 0)
      {
        throw new Exception($"{ArgsStrtuctureErrorMessage}: firstly should go argument which starts with '-'");
      }
    }

    return parsedArgs;
  }


  public class Parameters
  {
    public required string SourceDirPath { get; set; }
    public required string TargetDirPath { get; set; }
  }

  private static class Arguments
  {
    public const string SourceDir = "-sourceDir";
    public const string TargetDir = "-targetDir";
  }
}