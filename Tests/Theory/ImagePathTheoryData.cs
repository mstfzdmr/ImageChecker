using System.Collections.Generic;
using Xunit;

namespace Tests.Theory
{
    public class ImagePathTheoryData : TheoryData<List<string>>
    {
        public ImagePathTheoryData()
        {
            Add(new List<string>() { "ImageFilePathUrl" });
        }
    }

    public class ImagePathIsNullTheoryData : TheoryData<List<string>>
    {
        public ImagePathIsNullTheoryData()
        {
            Add(new List<string>());
        }
    }
}
