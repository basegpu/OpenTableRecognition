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
    }
}
