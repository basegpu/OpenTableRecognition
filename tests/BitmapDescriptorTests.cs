using System;
using Xunit;
using System.Drawing;
using OpenTableRegonition;

namespace Tests
{
    public class BitmapDescriptorTests
    {
        [Fact]
        public void SimpleBitmap()
        {
        	var bitmap = new Bitmap(5, 5);
            Assert.True(1 == 1, "dummy Test.");
        }
    }
}
