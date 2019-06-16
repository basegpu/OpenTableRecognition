using System;
using Xunit;
using OpenTableRegonition;
using SixLabors.ImageSharp;

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
            img.Save("images/outputGray.jpg");
        }

        [Fact]
        public void TestReadBlackWhite()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImageBlackWhite();
            Assert.True(img.Width == 2378);
            Assert.True(img.Height == 2422);
            img.Save("images/outputBW.jpg");
        }

        [Fact]
        public void TestReadBlackWhiteTreshold()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImageBlackWhite(0.25f);
            Assert.True(img.Width == 2378);
            Assert.True(img.Height == 2422);
            img.Save("images/outputBW025.jpg");
        }
    }
}
