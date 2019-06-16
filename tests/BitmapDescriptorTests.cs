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
        public void BooleanBitmap()
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

        [Fact]
        public void IntBitmap()
        {
            var bitmap = new int[2,3]{
                {0, 1, 2},
                {7, 8, 9}
            };
            var descriptor = new BitmapDescriptor<int>(bitmap);
            checkSize<int>(descriptor, 2, 3);
            checkHistogram(descriptor.RowHistogram, new double[]{3, 24});
            checkHistogram(descriptor.ColumnHistogram, new double[]{7, 9, 11});
        }

        [Fact]
        public void FloatBitmap()
        {
            var bitmap = new float[1,5]{
                {0.5f, 0.99f, 0.0f, 0.01f, 1.0f}
            };
            var descriptor = new BitmapDescriptor<float>(bitmap);
            checkSize<float>(descriptor, 1, 5);
            checkHistogram(descriptor.RowHistogram, new double[]{2.5});
            checkHistogram(descriptor.ColumnHistogram, new double[]{0.5, 0.99, 0.0, 0.01, 1.0});
        }

        [Fact]
        public void DoubleBitmap()
        {
            var bitmap = new double[4,1]{
                {0.5}, {0.99}, {1.0}, {0.01}
            };
            var descriptor = new BitmapDescriptor<double>(bitmap);
            checkSize<double>(descriptor, 4, 1);
            checkHistogram(descriptor.RowHistogram, new double[]{0.5, 0.99, 1.0, 0.01});
            checkHistogram(descriptor.ColumnHistogram, new double[]{2.5});
        }

        private void checkSize<T>(BitmapDescriptor<T> descriptor, int rows, int columns)
        {
            Assert.Equal(rows, descriptor.nRows);
            Assert.Equal(columns, descriptor.nColumns);
        }

        private void checkHistogram(double[] hist, double[] reference, int precision=6)
        {
            Assert.Equal(reference.Length, hist.Length);
            for (int ii = 0; ii < hist.Length; ii++)
            {
                Assert.Equal(reference[ii], hist[ii], precision);
            }
        }
    }
}
