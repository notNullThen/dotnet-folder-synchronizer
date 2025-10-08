namespace FoldersSynchronizer.Support;

public static class ArgumentsProcessor
{
  private const string ArgsStrtuctureErrorMessage = "Arguments structure is wrong";

  private static string[] _argumentsWithValues = [ConsoleParameters.SourceDirArgument, ConsoleParameters.TargetDirArgument];
  private static string[] _argumentsWithoutValues = [ConsoleParameters.DebugArgument];


  public static ConsoleParameters GetConsoleParameters(string[] args)
  {
    var parsedArgs = ParseArgs(args);

    var parameters = new ConsoleParameters();

    foreach (var arg in parsedArgs)
    {
      switch (arg.Key)
      {
        case ConsoleParameters.SourceDirArgument:
          parameters.SourceDirPath = arg.Value;
          continue;

        case ConsoleParameters.TargetDirArgument:
          parameters.TargetDirPath = arg.Value;
          continue;

        case ConsoleParameters.DebugArgument:
          parameters.DebugValue = true;
          continue;

        default: throw new Exception($"The entered '{arg.Key}' argument is wrong.");
      }
    }

    if (string.IsNullOrWhiteSpace(parameters.SourceDirPath))
    {
      throw new Exception($"You did not provided a '{ConsoleParameters.SourceDirArgument}' argument");
    }

    if (string.IsNullOrWhiteSpace(parameters.TargetDirPath))
    {
      throw new Exception($"You did not provided a '{ConsoleParameters.TargetDirArgument}' argument");
    }

    return parameters;
  }

  private static Dictionary<string, string> ParseArgs(string[] args)
  {
    var parsedArgs = new Dictionary<string, string>();

    for (var i = 0; i < args.Length; i++)
    {
      var arg = args[i];

      if (_argumentsWithoutValues.Any(argument => argument.Equals(arg)))
      {
        var key = arg;
        parsedArgs.Add(key, string.Empty);
        i++;
        arg = args[i];
      }


      if (_argumentsWithValues.Any(argument => argument.Contains(arg)))
      {
        var key = arg;
        var value = args[i + 1];

        if (value.StartsWith('-'))
        {
          throw new Exception($"{ArgsStrtuctureErrorMessage}: value of '{key}' argument starts with '-'");
        }

        parsedArgs.Add(key, value);

        i++;
      }
    }

    return parsedArgs;
  }
}