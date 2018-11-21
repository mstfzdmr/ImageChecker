using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageChecker.UnitTest
{
    [TestClass]
    public class ImageManagerTests
    {
        private readonly IImageManager _imageManager;
        private string imagePath;
        private bool imageCheckSuccessResult;
        private List<string> imagePaths;
        private List<string> notFoundedUrls;

        public ImageManagerTests()
        {
            _imageManager = new ImageManager();

            imagePath = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png";

            imagePaths = new List<string> {
                "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
                "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.jpg",
                "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.gif"
            };
        }

        [TestMethod]
        public void ImageCheck_With_Success_Return_IsTrue()
        {
            bool imageCheckSuccessResult = _imageManager.ImageCheck(imagePath);

            Assert.IsTrue(imageCheckSuccessResult);
        }

        [TestMethod]
        public void ImageCheck_With_Success_Return_NotFoundUrls_And_ImageCheckResult()
        {
            List<Tuple<string, bool>> imageCheckReturnUrlStatusResult = _imageManager.ImageCheck(imagePaths, out notFoundedUrls);

            Assert.IsTrue(notFoundedUrls.Any());
            Assert.IsTrue(imageCheckReturnUrlStatusResult.Any());
            Assert.IsNotNull(imageCheckReturnUrlStatusResult);
        }
    }
}
