using System.Runtime.InteropServices;

namespace ImageChecker.Helper
{
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope { get; set; }
        public ResourceTypeEnum ResourceType { get; set; }
        public ResourceDisplayTypeEnum DisplayType { get; set; }
        public int Usage { get; set; }
        public string LocalName { get; set; }
        public string RemoteName { get; set; }
        public string Comment { get; set; }
        public string Provider { get; set; }
    }
}
