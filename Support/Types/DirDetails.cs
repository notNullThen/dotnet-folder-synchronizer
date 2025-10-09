namespace FoldersSynchronizer.Support.Details;

public class DirDetails
{
  public string Name => System.IO.Path.GetFileName(Path);
  public required string Path { get; set; }
  public List<FileDetails> Files { get; set; }
  public List<DirDetails> Dirs { get; set; }
}
