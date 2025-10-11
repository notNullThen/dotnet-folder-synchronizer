using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileReceiver(ArgumentsParameters argumentsParameters) : FolderSynchronizerCore(argumentsParameters)
  {
    public void RecieveFiles()
    {
      if (!Directory.Exists(ArgumentsParameters.SourceDirPath)) throw new DirectoryNotFoundException(
        $"The Source Directory '{SourceDirDetails.Path}' was not found.\nPlease provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.");

      SourceDirDetails = GetDirDetails(ArgumentsParameters.SourceDirPath);

      if (Directory.Exists(ArgumentsParameters.TargetDirPath))
        TargetDirDetails = GetDirDetails(ArgumentsParameters.TargetDirPath);
      else
        Directory.CreateDirectory(ArgumentsParameters.TargetDirPath);
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