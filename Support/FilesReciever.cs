using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FilesReceiver(Logger logger)
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private List<FileDetails> _filesToIgnore;
  private List<FileDetails> _filesToProcess;

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };

  public void ImplementTasks()
  {
    // TODO: Implement function
  }

  public void IdentifyTasks()
  {
    List<FileDetails> filesToIgnore = new();
    List<FileDetails> filesToProcess = _targetDirDetails.Files.FindAll(targetFile =>
    {
      bool ignore = _sourceDirDetails.Files.Any(sourceFile => sourceFile.Name.Equals(targetFile.Name) && sourceFile.MD5.Equals(targetFile.MD5));
      bool process = !ignore;

      if (ignore)
      {
        filesToIgnore.Add(targetFile);
        logger.Log(targetFile, ignore, true);
      }

      return process;
    }).ToList();

    _filesToIgnore = filesToIgnore;
    _filesToProcess = filesToProcess;
  }

  public void RecieveFiles(ArgumentsParameters argumentParameters)
  {
    _sourceDirDetails = GetDirDetails(argumentParameters.SourceDirPath);

    // TODO: Finish error handling
    // throw new Exception($"The Source Directory '{_sourceDirDetails.Path}' was not found. Please provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.");

    if (Directory.Exists(argumentParameters.TargetDirPath))
      _targetDirDetails = GetDirDetails(argumentParameters.TargetDirPath);
    else
      Directory.CreateDirectory(argumentParameters.TargetDirPath);

    if (argumentParameters.DebugValue) Debug();
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
