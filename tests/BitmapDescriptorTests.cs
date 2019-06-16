using System;
using Xunit;
using System.Drawing;
using OpenTableRegonition;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests
{
    public class BitmapDescriptorTests
    {
        [Fact]
        public void SimpleBitmap()
        {
        	var bitmap = new Image<Rgba32>(5, 5);
            Assert.True(1 == 1, "dummy Test.");
        }
    }
}
