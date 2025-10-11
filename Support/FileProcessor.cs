using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support
{
  public class FileProcessor(ArgumentsParameters argumentsParameters) : FilesSynchronizerCore(argumentsParameters)
  {


    public bool AreFilesEqual(FileDetails sourceFile, FileDetails targetFile)
    {
      var sourceFileRelativePath = GetRelativePath(sourceFile.Path);
      var targetFileRelativePath = GetRelativePath(targetFile.Path);
      return sourceFileRelativePath.Equals(targetFileRelativePath) && sourceFile.MD5.Equals(targetFile.MD5);
    }
  }
}