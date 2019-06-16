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
            checkSize<bool>(descriptor, 0, 0);
            checkHistogram(descriptor.RowHistogram, new double[]{});
            checkHistogram(descriptor.ColumnHistogram, new double[]{});
        }

        [Fact]
        public void SimpleBitmap()
        {
            var bitmap = new bool[3,2]{
                {true,false},
                {false,false},
                {true,true}
            };
            var descriptor = new BitmapDescriptor<bool>(bitmap);
            checkSize<bool>(descriptor, 3, 2);
            checkHistogram(descriptor.RowHistogram, new double[]{1.0, 0.0, 2.0});
            checkHistogram(descriptor.ColumnHistogram, new double[]{2.0, 1.0});
        }

        private void checkSize<T>(BitmapDescriptor<T> descriptor, int rows, int columns)
        {
            Assert.Equal(rows, descriptor.nRows);
            Assert.Equal(columns, descriptor.nColumns);
        }

        private void checkHistogram(double[] hist, double[] reference)
        {
            Assert.Equal(reference.Length, hist.Length);
            for (int ii = 0; ii < hist.Length; ii++)
            {
                Assert.Equal(reference[ii], hist[ii]);
            }
        }
    }
}
