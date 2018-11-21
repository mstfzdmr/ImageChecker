using System;
using System.Collections.Generic;

namespace ImageChecker
{
    public interface IImageManager
    {
        bool ImageCheck(string imagePath);
        List<Tuple<string, bool>> ImageCheck(List<string> imagePaths, out List<string> notFoundedUrls);
    }
}
