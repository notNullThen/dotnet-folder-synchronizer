namespace FoldersSynchronizer.Support
{
  public static class Utils
  {
    public static string CalculateMD5FromFilePath(string filePath)
    {
      var fileBytes = File.ReadAllBytes(filePath);
      byte[] hashBytes = System.Security.Cryptography.MD5.HashData(fileBytes);
      return Convert.ToHexString(hashBytes);
    }

    public static string ParseMillisecondsToTimeString(int milliseconds)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
      return timeSpan.ToString(@"hh\:mm\:ss");
    }
  }
}