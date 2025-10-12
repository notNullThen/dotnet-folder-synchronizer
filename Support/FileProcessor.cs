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
        File.Copy(UseFullPath(relativePath, PathType.Source), UseFullPath(relativePath, PathType.Target));
    }

    public void PerformFilesDeletion()
    {
      foreach (var relativePath in _filesToDeleteRelativePaths)
        File.Delete(UseFullPath(relativePath, PathType.Target));
    }

    public void PerformFilesScan()
    {
      DetectFilesToDelete(sourceDirDetails, targetDirDetails);
      DetectFilesToCopy(sourceDirDetails, targetDirDetails);
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
          _filesToDeleteRelativePaths.Add(GetRelativePath(targetFile.Path));
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
          _filesToCopyRelativePaths.Add(GetRelativePath(sourceFile.Path));
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