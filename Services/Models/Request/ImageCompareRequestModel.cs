using System.Net;

namespace Models.Request
{
    public class ImageCompareRequestModel
    {
        public SourceRequest Source { get; set; }
        public TargetRequest Target { get; set; }
    }

    public class SourceRequest
    {
        public string FileName { get; set; }
        public string NetworkName { get; set; }
        public NetworkCredential NetworkCredential { get; set; }
    }

    public class TargetRequest
    {
        public string FileName { get; set; }
        public string NetworkName { get; set; }
        public NetworkCredential NetworkCredential { get; set; }
    }
}
