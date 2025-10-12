namespace FoldersSynchronizer.Support.Types;

public class FileDetails
{
  public required string Path { get; set; }
  public string Name => System.IO.Path.GetFileName(Path);
  public string MD5 => Utils.CalculateMD5FromFilePath(Path);
}