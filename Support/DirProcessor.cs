using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DirProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {
    private static readonly List<string> _dirsToDeletePaths = new();

    public static void PerformDirsDeletion()
    {
      foreach (var deletePath in _dirsToDeletePaths)
        Directory.Delete(deletePath, recursive: true);
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

        if (!matchFound) _dirsToDeletePaths.Add(targetSubDir.Path);
      }
    }

    private bool AreDirsEqual(DirDetails sourceDir, DirDetails targetDir)
    {
      var sourceDirRelativePath = GetRelativePath(sourceDir.Path);
      var targetDirRelativePath = GetRelativePath(targetDir.Path);
      return sourceDirRelativePath.Equals(targetDirRelativePath);
    }
  }
}