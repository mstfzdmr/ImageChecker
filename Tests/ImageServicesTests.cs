using ImageChecker;
using ImageChecker.Services;
using System;
using Xunit;

namespace Tests
{
    public class ImageServicesTests
    {
        [Theory, InlineData(new object[] { ""})]
        public void ImageCheck__ShouldArgumentNullException_WhenImagePathIsNull(string imagePath)
        {
            IImageServices imageServices = new ImageServices();
            var result = Record.Exception(() => imageServices.ImageCheck(imagePath));
            Assert.NotNull(result);
            var exception = Assert.IsType<ArgumentNullException>(result);
            var actual = exception.ParamName;
            const string expected = "imagePath";
            Assert.Equal(expected, actual);
        }
    }
}
