using ImageChecker;
using ImageChecker.Services;
using System;
using System.Collections.Generic;
using Tests.Theory;
using Xunit;

namespace Tests
{
    public class ImageServicesTests
    {
        [Theory, InlineData(new object[] { "" })]
        public void ImageCheck_WhenImagePathIsNull_ThenShouldBeArgumentNullException(string imagePath)
        {
            IImageServices imageServices = new ImageServices();
            var result = Record.Exception(() => imageServices.ImageCheck(imagePath));
            Assert.NotNull(result);
            var exception = Assert.IsType<ArgumentNullException>(result);
            var actual = exception.ParamName;
            const string expected = "imagePath";
            Assert.Equal(expected, actual);
        }

        [Theory, ClassData(typeof(ImagePathIsNullTheoryData))]
        public void ImageCheck_WhenImagePathsIsNull_ThenShouldBeArgumentNullException(List<string> imagePaths)
        {
            IImageServices imageServices = new ImageServices();
            var notFoundedUrls = new List<string>();
            var result = Record.Exception(() => imageServices.ImageCheck(imagePaths, out notFoundedUrls));
            Assert.NotNull(result);
            var exception = Assert.IsType<ArgumentNullException>(result);
            var actual = exception.ParamName;
            const string expected = "imagePaths";
            Assert.Equal(expected, actual);
        }
    }
}
