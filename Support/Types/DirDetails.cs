namespace FoldersSynchronizer.Support.Details;

public class DirDetails
{
  public string Path { get; set; }
  public List<FileDetails> Files { get; set; }
  public List<DirDetails> Dirs { get; set; }
}
