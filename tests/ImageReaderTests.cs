using System;
using Xunit;
using OpenTableRecognition;
using SixLabors.ImageSharp;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Tests
{
    public class ImageReaderTests
    {
        [Fact]
        public void WrongFile()
        {
            Action act = () => new ImageReader("images/dummy.png");
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void GetColorArray()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImageGrayscale();
            var colors = ImageReader.GetColorArray(img);
            var colorsSet = new HashSet<Rgba32>();
            for(var i = 0; i< img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    colorsSet.Add((Rgba32)colors.GetValue(i, j));
                }
            }
            Debug.WriteLine(string.Join(",", colorsSet.ToList()));
            Assert.True(colorsSet.All(x => x.B == x.G && x.G == x.R));
            
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

        [Fact]
        public void TestTIFConversion()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = ImageReader.Encode(imgReader.GetImageBlackWhite());

            byte[] buffer = new byte[1024 * 64];
            using (var file = File.OpenWrite("images/outputBW.tif"))
            {
                int read;
                while ((read = img.Read(buffer, 0, buffer.Length)) > 0)
                {

                    file.Write(buffer, 0, buffer.Length);
                }
            }
        }

        [Fact]
        public void TestTextExtract()
        {
            var imgReader = new ImageReader("images/receiptDark.jpg");
            var img = ImageReader.Encode(imgReader.GetImageBlackWhite());

            var bytes = new List<byte>();
            byte[] buffer = new byte[1024 * 64];
            
                int read;
                while ((read = img.Read(buffer, 0, buffer.Length)) > 0)
                {

                    bytes.AddRange(buffer);
                }

            var text = OcrProcessor.GetText(bytes.ToArray(), "deu");
            using (var f = new StreamWriter("images/ocrResultDarkDeu.txt"))
            {
                f.WriteLine(text);
            }
        }
    }
}
