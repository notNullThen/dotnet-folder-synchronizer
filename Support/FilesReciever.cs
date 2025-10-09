using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FilesReceiver(Logger logger, ArgumentsParameters argumentParameters)
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private readonly List<string> _filesToCopyPaths = [];
  private readonly List<string> _filesToIgnorePaths = [];
  private readonly List<string> _filesToDeletePaths = [];
  private readonly List<string> _dirsToDeletePaths = [];

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };

  public void ExecuteTasks()
  {
    if (argumentParameters.DebugValue) Debug();
  }

  public void CopyFolder()
  {
    // TODO: Implement
  }

  // private void GetTargetPaths()
  // {
  //   foreach (var fileToCopy in _filesToCopyPaths)
  // }

  public void DeleteDirs()
  {
    foreach (var dirToDelete in _dirsToDeletePaths)
    {
      try { Directory.Delete(dirToDelete); }
      catch
      {
        logger.LogError($"Could not delete '{dirToDelete}' dir. Details are below:\n");
        throw;
      }
    }
  }

  public void DeleteFiles()
  {
    foreach (var fileToDelete in _filesToDeletePaths)
    {
      try { File.Delete(fileToDelete); }
      catch
      {
        logger.LogError($"Could not delete '{fileToDelete}' file. Details are below:\n");
        throw;
      }
    }
  }

  public void ScanDir()
  {
    CheckDir(_sourceDirDetails, _targetDirDetails);
  }

  public void RecieveFiles()
  {
    if (!Directory.Exists(argumentParameters.SourceDirPath)) throw new DirectoryNotFoundException(
      $"The Source Directory '{_sourceDirDetails.Path}' was not found.\nPlease provide correct path in '{ArgumentsParameters.SourceDirArgument}' argument.");

    _sourceDirDetails = GetDirDetails(argumentParameters.SourceDirPath);

    if (Directory.Exists(argumentParameters.TargetDirPath))
      _targetDirDetails = GetDirDetails(argumentParameters.TargetDirPath);
    else
      Directory.CreateDirectory(argumentParameters.TargetDirPath);
  }

  public void Debug()
  {
    Console.WriteLine(@$"Files to replace:
{JsonSerializer.Serialize(_filesToDeletePaths, JsonPrettyOptions)}

Files to ignore:
{JsonSerializer.Serialize(_filesToIgnorePaths, JsonPrettyOptions)}

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
    var sourceFileToIgnore = sourceFiles.FirstOrDefault(sourceFile => AreFilesEqual(sourceFile, targetFile));

    if (sourceFileToIgnore != null)
    {
      _filesToIgnorePaths.Add(targetFile.Path);
      LogIgnoredFileDetails(targetFile);
    }
    else
    {
      // _filesToCopyPaths.Add(sourceFileToIgnore!.Path);
      _filesToDeletePaths.Add(targetFile.Path);
    }
  }

  private static bool AreFilesEqual(FileDetails sourceFile, FileDetails targetFile)
  {
    return sourceFile.Name.Equals(targetFile.Name) && sourceFile.MD5.Equals(targetFile.MD5);
  }

  private void LogIgnoredFileDetails(FileDetails fileDetails)
  {
    logger.LogInfo($"The target file \"{fileDetails.Name}\" will NOT be changed as it is identical with the source.\nPath: {fileDetails.Path}\n\n");
  }

  private void LogProcessedFileDetails(FileDetails fileDetails, bool ignored)
  {
    logger.LogSuccess($"The target file \"{fileDetails.Name}\" was changed.\nPath: {fileDetails.Path}\n\n");
  }
}
