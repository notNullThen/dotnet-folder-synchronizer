using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileProcessor(ArgumentsParameters argumentsParameters, Logger logger) : FilesSynchronizerCore(argumentsParameters, logger)
  {
    private static readonly List<string> _filesToDeleteRelativePaths = new();
    private static readonly List<string> _filesToCopyRelativePaths = new();

    public void PerformFilesCopying()
    {
      foreach (var relativePath in _filesToCopyRelativePaths)
      {
        var sourceCopyPath = UseFullPath(relativePath, PathType.Source);
        var targetCopyPath = UseFullPath(relativePath, PathType.Target);

        try
        {
          File.Copy(sourceCopyPath, targetCopyPath);
          logger.LogSuccess($"💾📄 The \"{sourceCopyPath}\" file is copied to target directory.");
        }
        catch (Exception exception)
        {
          logger.LogError($"Failed to copy \"{sourceCopyPath}\" file to target directory. Details are below:\n({exception.GetType().Name}): {exception.Message}");
          throw;
        }
      }
    }

    public void PerformFilesDeletion()
    {
      foreach (var relativePath in _filesToDeleteRelativePaths)
      {
        var targetDeletePath = UseFullPath(relativePath, PathType.Target);

        try
        {
          File.Delete(targetDeletePath);
          logger.LogSuccess($"🗑️📄 The \"{targetDeletePath}\" file is deleted.");
        }
        catch (Exception exception)
        {
          logger.LogError($"Failed to delete \"{targetDeletePath}\" file. Details are below:\n({exception.GetType().Name}): {exception.Message}");
          throw;
        }
      }
    }

    public void PerformFilesScan()
    {
      if (argumentsParameters.LogPreActionsValue)
        logger.LogInfo($"🔍📄 SCANNING TARGET DIRECTORY FOR FILES TO COPY/DELETE...");

      DetectFilesToCopy(sourceDirDetails, targetDirDetails);
      DetectFilesToDelete(sourceDirDetails, targetDirDetails);
    }

    private void DetectFilesToDelete(DirDetails sourceDir, DirDetails targetDir)
    {
      foreach (var sourceSubDir in sourceDir.Dirs)
      {
        foreach (var targetSubDir in targetDir.Dirs)
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            DetectFilesToDelete(sourceSubDir, targetSubDir);
            break;
          }
      }

      foreach (var targetFile in targetDir.Files)
      {
        bool matchFound = false;

        foreach (var sourceFile in sourceDir.Files)
        {
          if (AreFilesEqual(sourceFile, targetFile))
          {
            matchFound = true;
            break;
          }
        }

        if (!matchFound)
        {
          _filesToDeleteRelativePaths.Add(GetRelativePath(targetFile.Path));
          if (argumentsParameters.LogPreActionsValue)
          {
            if (_filesToCopyRelativePaths.Contains(GetRelativePath(targetFile.Path)))
              logger.LogInfo($"🔁📄 The \"{targetFile.Path}\" file will be replaced by source.");
            else
              logger.LogInfo($"🗑️📄 The \"{targetFile.Path}\" file will be deleted.");
          }
        }
        else
        {
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"⏩📄 The \"{targetFile.Path}\" file will not be touched as it is equal with a source one.");
        }
      }
    }

    private void DetectFilesToCopy(DirDetails sourceDir, DirDetails targetDir)
    {
      foreach (var targetSubDir in targetDir.Dirs)
      {
        foreach (var sourceSubDir in sourceDir.Dirs)
          if (AreDirsEqual(sourceSubDir, targetSubDir))
          {
            DetectFilesToCopy(sourceSubDir, targetSubDir);
            break;
          }
      }

      foreach (var sourceFile in sourceDir.Files)
      {
        bool matchFound = false;

        foreach (var targetFile in targetDir.Files)
        {
          if (AreFilesEqual(sourceFile, targetFile))
          {
            matchFound = true;
            break;
          }
        }

        if (!matchFound)
        {
          _filesToCopyRelativePaths.Add(GetRelativePath(sourceFile.Path));
          if (argumentsParameters.LogPreActionsValue)
            logger.LogInfo($"💾📄 The \"{sourceFile.Path}\" file will be copied.");
        }
        // Ignored files already logged in the deletion method
      }
    }

    private bool AreFilesEqual(FileDetails sourceFile, FileDetails targetFile)
    {
      var sourceFileRelativePath = GetRelativePath(sourceFile.Path);
      var targetFileRelativePath = GetRelativePath(targetFile.Path);
      return sourceFileRelativePath.Equals(targetFileRelativePath) && sourceFile.MD5.Equals(targetFile.MD5);
    }
  }
}