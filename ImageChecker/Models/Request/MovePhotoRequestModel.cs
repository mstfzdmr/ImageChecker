using System.Net;

namespace ImageChecker.Models.Request
{
    public class MovePhotoRequestModel
    {
        public NetworkCredential NetworkCredential { get; set; }
        public string ReadNetworkName { get; set; }
        public string WriteNetworkName { get; set; }
        public string WriteFilePath { get; set; }
        public string SourceFileName { get; set; }
        public string DestFileName { get; set; }
    }
}
