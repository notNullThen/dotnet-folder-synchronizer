using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {
    List<FileDetails> _filesToDelete;

    public void PerformFilesScan()
    {
      ScanFiles(sourceDirDetails.Files, targetDirDetails.Files);
    }

    private void ScanFiles(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
    {
      foreach (var sourceFile in sourceFiles)
      {
        foreach (var targetFile in targetFiles)
        {
          if (!AreFilesEqual(sourceFile, targetFile))
          {
            _filesToDelete.Add(targetFile);
          }
        }
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