using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public abstract class FolderSynchronizerCore(ArgumentsParameters argumentsParameters)
{
  protected ArgumentsParameters argumentsParameters = argumentsParameters;

  protected static DirDetails sourceDirDetails;
  protected static DirDetails targetDirDetails;

}
