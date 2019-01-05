using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageChecker.Services
{
    public interface IImageServices
    {
        bool ImageCheck(string imagePath);
        List<Tuple<string, bool>> ImageCheck(List<string> imagePaths, out List<string> notFoundedUrls);
        int CompareWith(Bitmap source, Bitmap target);
    }
}
