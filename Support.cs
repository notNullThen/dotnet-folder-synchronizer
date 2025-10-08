namespace FoldersSynchronizer;

public class Support
{
  public static DirDetails GetDirDetails(string dirPath)
  {
    var dirFilesPaths = Directory.GetFiles(dirPath);
    var subDirsPaths = Directory.GetDirectories(dirPath);

    var dirDetails = new DirDetails()
    {
      FilesDetails = dirFilesPaths.Select(GetFileDetails).ToList(),
      Dirs = subDirsPaths.Select(GetDirDetails).ToList()
    };

    dirDetails.Dirs = subDirsPaths.Select(GetDirDetails).ToList();

    return dirDetails;
  }

  private static FileDetails GetFileDetails(string filePath)
  {
    return new()
    {
      FilePath = filePath,
      Name = Path.GetFileName(filePath),
      MD5 = CalculateMD5FromFilePath(filePath)
    };
  }

  private static string CalculateMD5FromFilePath(string filePath)
  {
    var fileBytes = File.ReadAllBytes(filePath);
    byte[] hashBytes = System.Security.Cryptography.MD5.HashData(fileBytes);
    return Convert.ToHexString(hashBytes);
  }

  public class FileDetails
  {
    public required string FilePath { get; set; }
    public required string Name { get; set; }
    public required string MD5 { get; set; }
  }

  public class DirDetails
  {
    public string DirPath => Path.GetDirectoryName(FilesDetails.First().FilePath)!;
    public List<FileDetails> FilesDetails { get; set; }
    public List<DirDetails> Dirs { get; set; }
  }
}