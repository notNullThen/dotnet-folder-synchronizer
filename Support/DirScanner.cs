using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DirScanner(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {
    static readonly List<string> DirsToDeletePaths = new();

    public static void PerformFoldersDeletion()
    {
      foreach (var deletePath in DirsToDeletePaths)
      {
        Directory.Delete(deletePath, recursive: true);
      }
    }

    public void PerformFoldersScan()
    {
      if (Directory.Exists(argumentsParameters.TargetDirPath))
        ScanFolder(sourceDirDetails.Dirs, targetDirDetails.Dirs);
    }

    public void ScanFolder(List<DirDetails> sourceDir, List<DirDetails> targetDir)
    {
      foreach (var targetSubDir in targetDir)
      {
        bool matchFound = false;

        foreach (var sourceSubDir in sourceDir)
        {
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            matchFound = true;
            ScanFolder(sourceSubDir.Dirs, targetSubDir.Dirs);
            break;
          }
        }

        if (!matchFound) DirsToDeletePaths.Add(targetSubDir.Path);
      }
    }

    public bool AreFilesEqual(FileDetails sourceFile, FileDetails targetFile)
    {
      var sourceFileRelativePath = GetRelativePath(sourceFile.Path);
      var targetFileRelativePath = GetRelativePath(targetFile.Path);
      return sourceFileRelativePath.Equals(targetFileRelativePath) && sourceFile.MD5.Equals(targetFile.MD5);
    }

    public bool AreDirsEqual(DirDetails sourceDir, DirDetails targetDir)
    {
      var sourceDirRelativePath = GetRelativePath(sourceDir.Path);
      var targetDirRelativePath = GetRelativePath(targetDir.Path);
      return sourceDirRelativePath.Equals(targetDirRelativePath);
    }

    protected string GetRelativePath(string fullPath)
    {
      if (fullPath.StartsWith(argumentsParameters.SourceDirPath))
        fullPath = fullPath.Split(argumentsParameters.SourceDirPath)[1];
      if (fullPath.StartsWith(argumentsParameters.TargetDirPath))
        fullPath = fullPath.Split(argumentsParameters.TargetDirPath)[1];

      return fullPath;
    }
  }
}