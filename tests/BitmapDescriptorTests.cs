using System;
using Xunit;
using OpenTableRecognition;

namespace Tests
{
    public class BitmapDescriptorTests
    {
        [Fact]
        public void EmptyBitmap()
        {
        	var bitmap = new bool[,]{};
        	var descriptor = new BitmapDescriptor<bool>(bitmap);
            Assert.Equal(0, (int)descriptor.RowHistogram.Length);
            Assert.Equal(0, (int)descriptor.ColumnHistogram.Length);
        }
    }
}
