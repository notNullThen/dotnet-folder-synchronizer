namespace FoldersSynchronizer;

public static class ArgumentsHelper
{
  private const string ArgsStrtuctureErrorMessage = "Arguments structure is wrong";

  private static string[] _arguments = [ConsoleParameters.SourceDirAttribute, ConsoleParameters.TargetDirAttribute];


  public static ConsoleParameters GetConsoleParameters(string[] args)
  {
    var parsedArgs = ParseArgs(args);

    var parameters = new ConsoleParameters();

    foreach (var arg in parsedArgs)
    {
      switch (arg.Key)
      {
        case ConsoleParameters.SourceDirAttribute:
          parameters.SourceDirPath = arg.Value;
          continue;

        case ConsoleParameters.TargetDirAttribute:
          parameters.TargetDirPath = arg.Value;
          continue;

        case ConsoleParameters.DebugAttribute:
          parameters.DebugValue = true;
          continue;

        default: throw new Exception($"The entered '{arg.Key}' argument is wrong.");
      }
    }

    if (string.IsNullOrWhiteSpace(parameters.SourceDirPath))
    {
      throw new Exception($"You did not provided a '{ConsoleParameters.SourceDirAttribute}' argument");
    }

    if (string.IsNullOrWhiteSpace(parameters.TargetDirPath))
    {
      throw new Exception($"You did not provided a '{ConsoleParameters.TargetDirAttribute}' argument");
    }

    return parameters;
  }

  private static Dictionary<string, string> ParseArgs(string[] args)
  {
    var parsedArgs = new Dictionary<string, string>();

    for (var i = 0; i < args.Count(); i++)
    {
      var arg = args[i];

      if (args.Any(agr => _arguments.Any(argument => argument.Contains(arg))))
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
    }

    return parsedArgs;
  }


  public class ConsoleParameters
  {
    public const string SourceDirAttribute = "-sourceDir";
    public const string TargetDirAttribute = "-targetDir";
    public const string DebugAttribute = "-debug";
    public string SourceDirPath { get; set; }
    public string TargetDirPath { get; set; }
    public bool DebugValue { get; set; }
  }
}