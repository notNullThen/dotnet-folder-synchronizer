using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class DirProcessor(ArgumentsParameters argumentsParameters, Logger logger) : FilesSynchronizerCore(argumentsParameters, logger)
  {
    private static readonly List<string> _dirsToDeleteRelativePaths = new();

    public void PerformDirsDeletion()
    {
      foreach (var relativePath in _dirsToDeleteRelativePaths)
      {
        var targetPath = UseFullPath(relativePath, PathType.Target);
        try
        {
          Directory.Delete(targetPath, recursive: true);
          logger.LogSuccess($"The \"{targetPath}\" dir and files it contains are deleted.");
        }
        catch (Exception exception)
        {
          logger.LogError($"Failed to delete \"{targetPath}\" dir. Details are below:\n({exception.GetType().Name}): {exception.Message}");
          throw;
        }
      }
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
        else logger.LogInfo($"The \"{targetSubDir.Path}\" folder will not be touched as it has equal path with a source.");
      }
    }
  }
}