using FoldersSynchronizer.Support;
using FoldersSynchronizer.Support.Types;

namespace FoldersSynchronizer.Core
{
  public class DataReceiver(ArgumentsParameters argumentsParameters, Logger logger) : FilesSynchronizerCore(argumentsParameters, logger)
  {
    public void ReceiveData()
    {
      if (!Directory.Exists(argumentsParameters.SourceDirPath))
      {
        logger.LogError(
@$"The Source Directory '{sourceDirDetails.Path}' was not found.
Please provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.", true);
      }

      sourceDirDetails = GetDirDetails(argumentsParameters.SourceDirPath);

      if (Directory.Exists(argumentsParameters.TargetDirPath))
        targetDirDetails = GetDirDetails(argumentsParameters.TargetDirPath);
      else
      {
        targetDirDetails = new() { Path = argumentsParameters.TargetDirPath };
      }
    }

    private static DirDetails GetDirDetails(string dirPath)
    {
      var dirFilesPaths = Directory.GetFiles(dirPath);
      var subDirsPaths = Directory.GetDirectories(dirPath);

      var dirDetails = new DirDetails()
      {
        Path = dirPath,
        Files = dirFilesPaths.Select(filePath => new FileDetails() { Path = filePath }).ToList(),
        Dirs = subDirsPaths.Select(GetDirDetails).ToList()
      };

      dirDetails.Dirs = subDirsPaths.Select(GetDirDetails).ToList();

      return dirDetails;
    }
  }
}