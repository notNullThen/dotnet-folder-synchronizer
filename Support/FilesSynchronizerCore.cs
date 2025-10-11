using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public abstract class FilesSynchronizerCore(ArgumentsParameters argumentsParameters)
{
  protected ArgumentsParameters argumentsParameters = argumentsParameters;

  protected static DirDetails sourceDirDetails;
  protected static DirDetails targetDirDetails;

  protected string GetRelativePath(string fullPath)
  {
    if (fullPath.StartsWith(argumentsParameters.SourceDirPath))
      fullPath = fullPath.Split(argumentsParameters.SourceDirPath)[1];
    if (fullPath.StartsWith(argumentsParameters.TargetDirPath))
      fullPath = fullPath.Split(argumentsParameters.TargetDirPath)[1];

    return fullPath;
  }
}
