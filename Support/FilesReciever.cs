using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FilesReceiver(Logger logger, ArgumentsParameters argumentParameters)
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private readonly List<FileDetails> _filesToIgnore = [];
  private readonly List<FileDetails> _filesToReplace = [];
  private readonly List<string> _dirsToDeletePaths = [];

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };

  public void ExecuteTasks()
  {
    // TODO: Implement function
    if (argumentParameters.DebugValue)
      Debug();
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
          _dirsToDeletePaths.Add(targetSubdir.Path);
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
      _filesToReplace.Add(targetFile);
    }
  }

  public void RecieveFiles()
  {
    _sourceDirDetails = GetDirDetails(argumentParameters.SourceDirPath);

    // TODO: Finish error handling
    // throw new Exception($"The Source Directory '{_sourceDirDetails.Path}' was not found. Please provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.");

    if (Directory.Exists(argumentParameters.TargetDirPath))
      _targetDirDetails = GetDirDetails(argumentParameters.TargetDirPath);
    else
      Directory.CreateDirectory(argumentParameters.TargetDirPath);
  }

  public void Debug()
  {
    Console.WriteLine(@$"Files to replace:
{JsonSerializer.Serialize(_filesToReplace, JsonPrettyOptions)}

Files to ignore:
{JsonSerializer.Serialize(_filesToIgnore, JsonPrettyOptions)}

Dirs to delete:
{JsonSerializer.Serialize(_dirsToDeletePaths, JsonPrettyOptions)}

Source Dir details:
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
