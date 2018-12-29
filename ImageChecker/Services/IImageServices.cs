using System;
using System.Collections.Generic;

namespace ImageChecker.Services
{
    public interface IImageServices
    {
        bool ImageCheck(string imagePath);
        List<Tuple<string, bool>> ImageCheck(List<string> imagePaths, out List<string> notFoundedUrls);
    }
}
