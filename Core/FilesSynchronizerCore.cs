using FoldersSynchronizer.Support;
using FoldersSynchronizer.Support.Types;

namespace FoldersSynchronizer.Core;

public abstract class FilesSynchronizerCore(ArgumentsParameters argumentsParameters, Logger logger)
{
  protected readonly ArgumentsParameters argumentsParameters = argumentsParameters;
  protected readonly Logger logger = logger;

  protected static DirDetails sourceDirDetails;
  protected static DirDetails targetDirDetails;

  protected static List<string> _filesToDeleteRelativePaths = new();
  protected static List<string> _filesToCopyRelativePaths = new();

  protected enum PathType
  {
    Source,
    Target
  }

  public void EraseData()
  {
    sourceDirDetails = new() { Path = argumentsParameters.SourceDirPath };
    targetDirDetails = new() { Path = argumentsParameters.TargetDirPath };
    _filesToDeleteRelativePaths = new();
    _filesToCopyRelativePaths = new();
  }

  protected string UseFullPath(string relativePath, PathType pathType)
  {
    if (pathType == PathType.Source)
      return argumentsParameters.SourceDirPath + relativePath;
    else
      return argumentsParameters.TargetDirPath + relativePath;
  }
  protected string GetRelativePath(string fullPath)
  {
    if (fullPath.StartsWith(argumentsParameters.SourceDirPath))
      fullPath = fullPath.Split(argumentsParameters.SourceDirPath)[1];
    if (fullPath.StartsWith(argumentsParameters.TargetDirPath))
      fullPath = fullPath.Split(argumentsParameters.TargetDirPath)[1];

    return fullPath;
  }

  protected bool AreDirsEqual(DirDetails sourceDir, DirDetails targetDir)
  {
    var sourceDirRelativePath = GetRelativePath(sourceDir.Path);
    var targetDirRelativePath = GetRelativePath(targetDir.Path);
    return sourceDirRelativePath.Equals(targetDirRelativePath);
  }
}
