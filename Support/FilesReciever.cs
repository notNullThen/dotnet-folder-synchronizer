using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FilesReceiver
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };


  public void RecieveFiles(ConsoleParameters parameters)
  {

    _sourceDirDetails = GetDirDetails(parameters.SourceDirPath);

    if (Directory.Exists(parameters.TargetDirPath))
      _targetDirDetails = GetDirDetails(parameters.TargetDirPath);
    else
      Directory.CreateDirectory(parameters.TargetDirPath);

    if (parameters.DebugValue) Debug();
  }

  public void Debug()
  {
    Console.WriteLine(@$"Source Dir details:
{JsonSerializer.Serialize(_sourceDirDetails, JsonPrettyOptions)}

Target Dir Details:
{JsonSerializer.Serialize(_targetDirDetails, JsonPrettyOptions)}
");
  }

  public static DirDetails GetDirDetails(string dirPath)
  {
    var dirFilesPaths = Directory.GetFiles(dirPath);
    var subDirsPaths = Directory.GetDirectories(dirPath);

    var dirDetails = new DirDetails()
    {
      Path = dirPath,
      Files = dirFilesPaths.Select(GetFileDetails).ToList(),
      Dirs = subDirsPaths.Select(GetDirDetails).ToList()
    };

    dirDetails.Dirs = subDirsPaths.Select(GetDirDetails).ToList();

    return dirDetails;
  }

  private static FileDetails GetFileDetails(string filePath)
  {
    return new()
    {
      Path = filePath,
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
}
