namespace FoldersSynchronizer.Support.Types;

public class DirDetails
{
  public string Name => System.IO.Path.GetFileName(Path);
  public required string Path { get; set; }
  public List<FileDetails> Files { get; set; } = new();
  public List<DirDetails> Dirs { get; set; } = new();
}
