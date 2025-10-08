namespace FoldersSynchronizer.Support;

public static class ArgumentsProcessor
{
  private const string ArgsStrtuctureErrorMessage = "Arguments structure is wrong";

  private static string[] _argumentsWithValues = [
    ArgumentsParameters.SourceDirArgument,
    ArgumentsParameters.TargetDirArgument,
    ArgumentsParameters.LogsArgument];
  private static string[] _argumentsWithoutValues = [ArgumentsParameters.DebugArgument];


  public static ArgumentsParameters GetParametersFromArguments(string[] args)
  {
    var parsedArgs = ParseArgs(args);

    var parameters = new ArgumentsParameters();

    foreach (var arg in parsedArgs)
    {
      switch (arg.Key)
      {
        case ArgumentsParameters.SourceDirArgument:
          parameters.SourceDirPath = arg.Value;
          continue;

        case ArgumentsParameters.TargetDirArgument:
          parameters.TargetDirPath = arg.Value;
          continue;

        case ArgumentsParameters.DebugArgument:
          parameters.DebugValue = true;
          continue;

        case ArgumentsParameters.LogsArgument:
          parameters.LogsFilePath = arg.Value;
          continue;

        default: throw new Exception($"The entered '{arg.Key}' argument is wrong.");
      }
    }

    if (string.IsNullOrWhiteSpace(parameters.SourceDirPath))
    {
      throw new Exception($"You did not provide the '{ArgumentsParameters.SourceDirArgument}' argument");
    }

    if (string.IsNullOrWhiteSpace(parameters.TargetDirPath))
    {
      throw new Exception($"You did not provide the '{ArgumentsParameters.TargetDirArgument}' argument");
    }

    if (string.IsNullOrWhiteSpace(parameters.LogsFilePath))
    {
      throw new Exception($"You did not provide the '{ArgumentsParameters.LogsArgument}' argument");
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