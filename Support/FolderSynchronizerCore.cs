using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public abstract class FolderSynchronizerCore(ArgumentsParameters argumentsParameters)
{
  protected ArgumentsParameters ArgumentsParameters = argumentsParameters;

  protected static DirDetails SourceDirDetails;
  protected static DirDetails TargetDirDetails;

}
