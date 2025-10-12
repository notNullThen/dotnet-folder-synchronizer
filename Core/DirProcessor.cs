using FoldersSynchronizer.Support;
using FoldersSynchronizer.Support.Types;

namespace FoldersSynchronizer.Core
{
  public class DirProcessor(ArgumentsParameters argumentsParameters, Logger logger) : FilesSynchronizerCore(argumentsParameters, logger)
  {
    private static readonly List<string> _dirsToDeleteRelativePaths = new();
    private static readonly List<string> _dirsToCreateRelativePaths = new();

    public void PerformDirsCreation()
    {
      logger.Log(@"
-------------------------------
🗑️📁 CREATING DIRS STARTED...
-------------------------------");

      foreach (var relativePath in _dirsToCreateRelativePaths)
      {
        var targetPath = UseFullPath(relativePath, PathType.Target);
        try
        {
          Directory.CreateDirectory(targetPath);
          logger.LogSuccess($"🗑️📁 The \"{targetPath}\" dir is created.");
        }
        catch (Exception exception)
        {
          logger.LogError($"Failed to create \"{targetPath}\" dir. Details are below:\n({exception.GetType().Name}): {exception.Message}");
          throw;
        }
      }
    }

    public void PerformDirsDeletion()
    {
      logger.Log(@"
-------------------------------
🗑️📁 DELETING DIRS STARTED...
-------------------------------");

      foreach (var relativePath in _dirsToDeleteRelativePaths)
      {
        var targetPath = UseFullPath(relativePath, PathType.Target);
        try
        {
          if (!Directory.Exists(targetPath))
          {
            logger.LogAlert($"⏩📁 The \"{targetPath}\" dir already does not exist, so no need to delete it.");
            continue;
          }
          Directory.Delete(targetPath, recursive: true);
          logger.LogSuccess($"🗑️📁 The \"{targetPath}\" dir and files it contains are deleted.");
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
      if (argumentsParameters.LogPreActionsValue)
        logger.Log(@"
--------------------------------------------------------------
🔍📁 SCANNING TARGET DIRECTORY FOR DIRS TO DELETE STARTED...
--------------------------------------------------------------");

      if (!Directory.Exists(argumentsParameters.TargetDirPath))
      {
        Directory.CreateDirectory(argumentsParameters.TargetDirPath);
        logger.LogAlert($"➕📁 The target dir \"{argumentsParameters.TargetDirPath}\" did not exist, so it was created.");
      }

      DetectDirsToDelete(sourceDirDetails.Dirs, targetDirDetails.Dirs);
      DetectDirsToCreate(sourceDirDetails.Dirs, targetDirDetails.Dirs);
    }

    private void DetectDirsToDelete(List<DirDetails> sourceDirs, List<DirDetails> targetDirs)
    {
      foreach (var targetSubDir in targetDirs)
      {
        bool matchFound = false;

        foreach (var sourceSubDir in sourceDirs)
        {
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            matchFound = true;
            DetectDirsToDelete(sourceSubDir.Dirs, targetSubDir.Dirs);
            break;
          }
        }

        if (!matchFound)
        {
          _dirsToDeleteRelativePaths.Add(targetSubDir.Path);
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"🗑️📁 The \"{targetSubDir.Path}\" folder will be deleted.");
        }
        else
        {
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"⏩📁 The \"{targetSubDir.Path}\" folder will not be touched as it has equal path with a source one.");
        }
      }
    }

    private void DetectDirsToCreate(List<DirDetails> sourceDirs, List<DirDetails> targetDirs)
    {
      foreach (var sourceSubDir in sourceDirs)
      {
        bool matchFound = false;

        foreach (var targetSubDir in targetDirs)
        {
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            matchFound = true;
            DetectDirsToCreate(sourceSubDir.Dirs, targetSubDir.Dirs);
            break;
          }
        }

        if (!matchFound)
        {
          _dirsToCreateRelativePaths.Add(GetRelativePath(sourceSubDir.Path));
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"🗑️📁 The \"{sourceSubDir.Path}\" folder will be deleted.");

          targetDirs.Add(new() { Path = UseFullPath(GetRelativePath(sourceSubDir.Path), PathType.Target) });
          DetectDirsToCreate(sourceSubDir.Dirs, targetDirs.Last().Dirs);
        }
        else
        {
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"⏩📁 The \"{sourceSubDir.Path}\" folder will not be touched as it has equal path with a source one.");
        }
      }
    }
  }
}