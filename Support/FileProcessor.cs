using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {
    private static readonly List<string> _filesToProcessRelativePaths = new();

    public static void PerformFilesCopying()
    {

    }

    public void PerformFilesDeletion()
    {
      foreach (var relativeDeletePath in _filesToProcessRelativePaths)
        File.Delete(UseFullPath(relativeDeletePath, PathType.Target));
    }

    public void PerformFilesScan()
    {
      ScanFiles(sourceDirDetails.Files, targetDirDetails.Files);
    }

    private void ScanFiles(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
    {
      foreach (var targetFile in targetFiles)
      {
        bool matchFound = false;

        foreach (var sourceFile in sourceFiles)
        {
          if (AreFilesEqual(sourceFile, targetFile))
          {
            matchFound = true;
            break;
          }
        }

        if (!matchFound) _filesToProcessRelativePaths.Add(GetRelativePath(targetFile.Path));
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