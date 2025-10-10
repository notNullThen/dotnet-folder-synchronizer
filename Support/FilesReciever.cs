using System.Text.Json;
using FoldersSynchronizer.Support.Details;

namespace FoldersSynchronizer.Support;

public class FolderSynchronizerCore(Logger logger, ArgumentsParameters argumentParameters)
{
  private DirDetails _sourceDirDetails;
  private DirDetails _targetDirDetails;

  private readonly List<string> _filesToCopyRelativePaths = [];
  private readonly List<string> _filesToIgnoreRelativePaths = [];
  private readonly List<string> _filesToDeleteRelativePaths = [];
  private readonly List<string> _dirsToDeleteRelativePaths = [];

  private static readonly JsonSerializerOptions JsonPrettyOptions = new() { WriteIndented = true };

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

  public void ScanDir()
  {
    CheckDir(_sourceDirDetails, _targetDirDetails);
  }

  public void CopyFiles()
  {
    foreach (var relativePath in _filesToCopyRelativePaths)
    {
      var sourceFullPath = argumentParameters.SourceDirPath + relativePath;
      var targetFullPath = argumentParameters.TargetDirPath + relativePath;

      if (File.Exists(targetFullPath)) logger.LogError($"The '{targetFullPath}' file we want to copy already exists.", true);

      File.Copy(sourceFullPath, targetFullPath);
    }
  }

  public void DeleteTargetDirs()
  {
    foreach (var dirToDelete in _dirsToDeleteRelativePaths)
    {
      var fullPath = Path.Combine(argumentParameters.TargetDirPath, dirToDelete);

      if (!Directory.Exists(fullPath)) logger.LogInfo($"The '{fullPath}' directory we want to delete already does not exists.");

      try { Directory.Delete(fullPath); }
      catch
      {
        logger.LogError($"Could not delete the '{fullPath}' dir. Details are below:\n");
        throw;
      }
    }
  }

  public void DeleteTargetFiles()
  {
    foreach (var fileToDelete in _filesToDeleteRelativePaths)
    {
      var fullPath = Path.Combine(argumentParameters.TargetDirPath, fileToDelete);

      if (!File.Exists(fullPath)) logger.LogInfo($"The '{fullPath}' file we want to delete already does not exists.");

      try { File.Delete(fileToDelete); }
      catch
      {
        logger.LogError($"Could not delete the '{fileToDelete}' file. Details are below:\n");
        throw;
      }
    }
  }

  public void Debug()
  {
    Console.WriteLine(@$"Files to replace:
{JsonSerializer.Serialize(_filesToDeleteRelativePaths, JsonPrettyOptions)}

Files to ignore:
{JsonSerializer.Serialize(_filesToIgnoreRelativePaths, JsonPrettyOptions)}

Dirs to delete:
{JsonSerializer.Serialize(_dirsToDeleteRelativePaths, JsonPrettyOptions)}

Source Dir details:
{JsonSerializer.Serialize(_sourceDirDetails, JsonPrettyOptions)}

Target Dir Details:
{JsonSerializer.Serialize(_targetDirDetails, JsonPrettyOptions)}
");
  }

  private string GetRelativePath(string fullPath)
  {
    if (fullPath.StartsWith(argumentParameters.SourceDirPath))
      fullPath = fullPath.Split(argumentParameters.SourceDirPath)[1];
    if (fullPath.StartsWith(argumentParameters.TargetDirPath))
      fullPath = fullPath.Split(argumentParameters.TargetDirPath)[1];

    return fullPath;
  }

  private static DirDetails GetDirDetails(string dirPath)
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
        if (!AreDirsEqual(sourceSubDir, targetSubdir))
        {
          _dirsToDeleteRelativePaths.Add(GetRelativePath(targetSubdir.Path));
          continue;
        }

        CheckDir(sourceSubDir, targetSubdir);
      }

    foreach (var targetFile in targetDir.Files)
      CheckFile(sourceDir.Files, targetFile);
  }

  private void CheckFile(List<FileDetails> sourceFiles, FileDetails targetFile)
  {
    FileDetails? sourceFileToCopy = null;

    bool ignore = sourceFiles.Any(sourceFile =>
    {
      if (AreFilesEqual(sourceFile, targetFile))
      {
        return true;
      }

      sourceFileToCopy = sourceFile;
      return false;
    });

    if (ignore)
    {
      if (sourceFileToCopy != null)
      {
        _filesToCopyRelativePaths.Add(GetRelativePath(sourceFileToCopy.Path));
      }

      _filesToIgnoreRelativePaths.Add(GetRelativePath(targetFile.Path));
      logger.LogInfo($"The target file \"{targetFile.Name}\" will NOT be changed as it is identical with the source.\nPath: {targetFile.Path}\n\n");
    }
    else
    {
      _filesToDeleteRelativePaths.Add(GetRelativePath(targetFile.Path));
    }
  }

  private static bool AreFilesEqual(FileDetails sourceFile, FileDetails targetFile)
  {
    return sourceFile.Name.Equals(targetFile.Name) && sourceFile.MD5.Equals(targetFile.MD5);
  }

  private static bool AreDirsEqual(DirDetails sourceDir, DirDetails targetDir)
  {
    return sourceDir.Name.Equals(targetDir.Name);
  }
}
