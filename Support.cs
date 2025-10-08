namespace FoldersSynchronizer;

public class Support
{
  public static DirDetails GetFilesFromDir(string dirPath)
  {
    var dirContent = Directory.GetFiles(dirPath);

    var dirDetails = GetDirDetails(dirContent);

    return dirDetails;
  }

  private static DirDetails GetDirDetails(string[] dirContent)
  {
    var dirFiles = dirContent.Select(GetFileDetails).ToList();

    // TODO: Implement recursive dir detection and details getting

    return new DirDetails
    {
      Files = dirFiles,
    };
  }

  private static FileDetails GetFileDetails(string filePath)
  {
    var fileContent = File.ReadAllBytes(filePath);

    return new FileDetails()
    {
      FilePath = filePath,
      MD5 = CalculateMD5(fileContent)
    };
  }

  private static string CalculateMD5(byte[] input)
  {
    byte[] hashBytes = System.Security.Cryptography.MD5.HashData(input);
    return Convert.ToHexString(hashBytes);
  }

  public class FileDetails
  {
    public string FilePath { get; set; }
    public string Name => Path.GetFileName(FilePath);

    public string MD5 { get; set; }
  }

  public class DirDetails
  {
    public List<FileDetails> Files { get; set; }
    public List<DirDetails> Dirs { get; set; }
  }
}