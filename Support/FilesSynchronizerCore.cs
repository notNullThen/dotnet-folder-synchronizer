using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public abstract class FilesSynchronizerCore(ArgumentsParameters argumentsParameters)
{
  protected ArgumentsParameters argumentsParameters = argumentsParameters;

  protected static DirDetails sourceDirDetails;
  protected static DirDetails targetDirDetails;

}
