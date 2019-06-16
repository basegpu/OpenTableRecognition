using System;
using Xunit;
using OpenTableRegonition;

namespace Tests
{
    public class ImageReaderTests
    {
        [Fact]
        public void PrintStringWithColor()
        {
            var reader = new ImageReader("images/receipt.jpg");
            Assert.True(1 == 1, "dummy Test.");
        }

        [Fact]
        public void TestRead()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImage();
            Assert.True(img.Width == 2378);
            Assert.True(img.Height == 2422);
        }

        [Fact]
        public void TestReadGrayscale()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImageGrayscale();
            Assert.True(img.Width == 2378);
            Assert.True(img.Height == 2422);
            
        }
    }
}
