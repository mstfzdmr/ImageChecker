using System.Linq;
using System.Text.RegularExpressions;

namespace ImageChecker.Extensions
{
    public static class FolderExtensions
    {
        public static string CreateFilePath(string filePath, string fileName, int filePathSize)
        {
            var filePaths = Regex.Split(fileName.Substring(0, filePathSize).ToLower(), string.Empty).Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
            string result = filePaths.Aggregate(string.Empty, (current, item) => current + (item + @"\"));
            return $@"{filePath}\{result}";
        }
    }
}
