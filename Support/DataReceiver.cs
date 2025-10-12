using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DataReceiver(ArgumentsParameters argumentsParameters, Logger logger) : FilesSynchronizerCore(argumentsParameters, logger)
  {
    public void ReceiveData()
    {
      if (!Directory.Exists(argumentsParameters.SourceDirPath)) throw new DirectoryNotFoundException(
        $"The Source Directory '{sourceDirDetails.Path}' was not found.\nPlease provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.");

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