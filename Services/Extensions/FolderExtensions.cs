using ImageChecker.Helper;
using System.Drawing;
using System.Linq;
using System.Net;
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
        public static Bitmap GetBitmap(string fileName, string networkName, NetworkCredential networkCredential)
        {
            Bitmap bitmap;
            using (new NetworkConnection(networkName, networkCredential))
            {
                bitmap = new Bitmap(fileName);
            }

            return bitmap;
        }
    }
}
