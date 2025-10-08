namespace FoldersSynchronizer;

public static class ArgumentsHelper
{
  private const string ArgsStrtuctureErrorMessage = "Arguments structure is wrong";


  public static Parameters GetParameters(string[] args)
  {
    var parsedArgs = ParseArgs(args);

    var parameters = new Parameters();

    foreach (var arg in parsedArgs)
    {
      switch (arg.Key)
      {
        case Arguments.SourceDir:
          parameters.SourceDirPath = arg.Value;
          continue;

        case Arguments.TargetDir:
          parameters.TargetDirPath = arg.Value;
          continue;

        default: throw new Exception($"The entered '{arg.Key}' argument is wrong.");
      }
    }

    if (string.IsNullOrWhiteSpace(parameters.SourceDirPath))
    {
      throw new Exception($"You did not provided a '{Arguments.SourceDir}' argument");
    }

    if (string.IsNullOrWhiteSpace(parameters.TargetDirPath))
    {
      throw new Exception($"You did not provided a '{Arguments.TargetDir}' argument");
    }

    return parameters;
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
    public string SourceDirPath { get; set; }
    public string TargetDirPath { get; set; }
  }

  private static class Arguments
  {
    public const string SourceDir = "-sourceDir";
    public const string TargetDir = "-targetDir";
  }
}