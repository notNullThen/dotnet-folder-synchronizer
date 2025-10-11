using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
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
      DetectFilesToDelete(sourceDirDetails.Files, targetDirDetails.Files);
      DetectFilesToCopy(sourceDirDetails.Files, targetDirDetails.Files);
    }

    private void DetectFilesToDelete(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
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

        if (!matchFound)
          _filesToDeleteRelativePaths.Add(GetRelativePath(targetFile.Path));
      }
    }

    private void DetectFilesToCopy(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
    {
      foreach (var sourceFile in sourceFiles)
      {
        bool matchFound = false;

        foreach (var targetFile in targetFiles)
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