using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DirProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {
    private static readonly List<string> _dirsToDeleteRelativePaths = new();

    public void PerformDirsDeletion()
    {
      foreach (var relativePath in _dirsToDeleteRelativePaths)
        Directory.Delete(UseFullPath(relativePath, PathType.Target), recursive: true);
    }

    public void PerformDirsScan()
    {
      ScanDir(sourceDirDetails.Dirs, targetDirDetails.Dirs);
    }

    private void ScanDir(List<DirDetails> sourceDir, List<DirDetails> targetDir)
    {
      foreach (var targetSubDir in targetDir)
      {
        bool matchFound = false;

        foreach (var sourceSubDir in sourceDir)
        {
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            matchFound = true;
            ScanDir(sourceSubDir.Dirs, targetSubDir.Dirs);
            break;
          }
        }

        if (!matchFound) _dirsToDeleteRelativePaths.Add(targetSubDir.Path);
      }
    }
  }
}