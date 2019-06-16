using OpenTableRegonition;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace tests
{
    public class ReadImage
    {
        [Fact]
        public void TestRead()
        {
            var imgReader = new ImageReader("images/receipt.jpg");
            var img = imgReader.GetImage();
            Assert.True(img.Width == 2378);
            Assert.True(img.Height == 2422);
        }
        
    }
}
