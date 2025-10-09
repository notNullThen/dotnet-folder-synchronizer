using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FilesReceiver(Logger logger)
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private readonly List<FileDetails> _filesToIgnore = [];
  private readonly List<FileDetails> _filesToProcess = [];
  private readonly List<string> _dirsToDeletePaths = [];

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };

  public void ExecuteTasks()
  {
    // TODO: Implement function
  }

  public void ScanDir()
  {
    CheckDir(_sourceDirDetails, _targetDirDetails);
  }

  private void CheckDir(DirDetails sourceDir, DirDetails targetDir)
  {
    foreach (var sourceSubDir in sourceDir.Dirs)
      foreach (var targetSubdir in targetDir.Dirs)
      {
        if (!sourceSubDir.Name.Equals(targetSubdir.Name))
        {
          _dirsToDeletePaths.Add(targetDir.Path);
          continue;
        }

        CheckDir(sourceSubDir, targetSubdir);
      }

    foreach (var targetFile in targetDir.Files)
      CheckFile(sourceDir.Files, targetFile);
  }

  private void CheckFile(List<FileDetails> sourceFiles, FileDetails targetFile)
  {
    bool ignore = sourceFiles.Any(sourceFile => sourceFile.Name.Equals(targetFile.Name) && sourceFile.MD5.Equals(targetFile.MD5));

    if (ignore)
    {
      _filesToIgnore.Add(targetFile);
      logger.Log(targetFile, ignore, true);
    }
    else
    {
      _filesToProcess.Add(targetFile);
    }
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
      Files = dirFilesPaths.Select(filePath => new FileDetails() { Path = filePath }).ToList(),
      Dirs = subDirsPaths.Select(GetDirDetails).ToList()
    };

    dirDetails.Dirs = subDirsPaths.Select(GetDirDetails).ToList();

    return dirDetails;
  }
}
