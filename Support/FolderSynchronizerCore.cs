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
      var fullPath = argumentParameters.TargetDirPath + dirToDelete;

      if (!Directory.Exists(fullPath)) logger.LogInfo($"The '{fullPath}' directory we want to delete already does not exists.");

      try { Directory.Delete(fullPath, true); }
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
      var fullPath = argumentParameters.TargetDirPath + fileToDelete;

      if (!File.Exists(fullPath)) logger.LogInfo($"The '{fullPath}' file we want to delete already does not exists.");

      try { File.Delete(fullPath); }
      catch
      {
        logger.LogError($"Could not delete the '{fullPath}' file. Details are below:\n");
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
    CheckIgnoreFiles(sourceDir.Files, targetDir.Files);
    CheckDeleteFiles(sourceDir.Files, targetDir.Files);
    CheckCopyFiles(sourceDir.Files, targetDir.Files);

    foreach (var sourceSubDir in sourceDir.Dirs)
      foreach (var targetSubDir in targetDir.Dirs)
      {
        if (!AreDirsEqual(sourceSubDir, targetSubDir))
        {
          _dirsToDeleteRelativePaths.Add(GetRelativePath(targetSubDir.Path));
          continue;
        }

        CheckDir(sourceSubDir, targetSubDir);
      }
  }

  private void CheckCopyFiles(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
  {
    if (_filesToIgnoreRelativePaths.Count == 0) throw new Exception($"Use the {nameof(CheckDeleteFiles)}() only after {nameof(CheckIgnoreFiles)}() is done. This function uses {nameof(_filesToIgnoreRelativePaths)}");

    foreach (var sourceFile in sourceFiles)
    {
      foreach (var targetFile in targetFiles)
      {
        bool isIgnored = _filesToIgnoreRelativePaths.Any(ignoreRelativePath => ignoreRelativePath == GetRelativePath(sourceFile.Path));

        if (!AreFilesEqual(sourceFile, targetFile) && !isIgnored)
        {
          _filesToCopyRelativePaths.Add(GetRelativePath(sourceFile.Path));
          break;
        }
      }
    }
  }


  private void CheckDeleteFiles(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
  {
    if (_filesToIgnoreRelativePaths.Count == 0) throw new Exception($"Use the {nameof(CheckDeleteFiles)}() only after {nameof(CheckIgnoreFiles)}() is done. This function uses {nameof(_filesToIgnoreRelativePaths)} and it's empty now.");

    foreach (var targetFile in targetFiles)
    {
      foreach (var sourceFile in sourceFiles)
      {
        bool isIgnored = _filesToIgnoreRelativePaths.Any(ignoreRelativePath => ignoreRelativePath == GetRelativePath(targetFile.Path));

        if (!AreFilesEqual(sourceFile, targetFile) && !isIgnored)
        {
          _filesToDeleteRelativePaths.Add(GetRelativePath(targetFile.Path));
          break;
        }
      }
    }
  }

  private void CheckIgnoreFiles(List<FileDetails> sourceFiles, List<FileDetails> targetFiles)
  {
    foreach (var targetFile in targetFiles)
    {
      foreach (var sourceFile in sourceFiles)
      {
        if (AreFilesEqual(sourceFile, targetFile))
        {
          _filesToIgnoreRelativePaths.Add(GetRelativePath(sourceFile.Path));
          break;
        }
      }
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
