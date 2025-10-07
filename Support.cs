namespace FoldersSynchronizer;

public class Support
{
  public static List<FileDetails> GetFilesFromDir(string dirPath)
  {
    var dirContent = Directory.GetFiles(dirPath);

    var dirDetails = GetDirDetails(dirContent);

    return dirDetails;
  }

  private static List<FileDetails> GetDirDetails(string[] dirContent)
  {
    return dirContent.Select(GetFileDetails).ToList();
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
    public required string FilePath { get; set; }
    public string Name => Path.GetFileName(FilePath);

    public required string MD5 { get; set; }
  }
}