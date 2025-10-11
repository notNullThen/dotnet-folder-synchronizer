using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DirProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
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

    private void ScanFolder(List<DirDetails> sourceDir, List<DirDetails> targetDir)
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

    private bool AreDirsEqual(DirDetails sourceDir, DirDetails targetDir)
    {
      var sourceDirRelativePath = GetRelativePath(sourceDir.Path);
      var targetDirRelativePath = GetRelativePath(targetDir.Path);
      return sourceDirRelativePath.Equals(targetDirRelativePath);
    }
  }
}