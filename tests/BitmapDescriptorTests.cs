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
            checkHistogram(descriptor.RowHistogram, new double[]{0.3333, 0.0, 0.6666}, 3);
            checkHistogram(descriptor.ColumnHistogram, new double[]{0.6666, 0.3333}, 3);
        }

        [Fact]
        public void IntBitmap()
        {
            var bitmap = new int[2,3]{
                {0, 1, 2},
                {1, 4, 2}
            };
            var descriptor = new BitmapDescriptor<int>(bitmap);
            checkSize<int>(descriptor, 2, 3);
            checkHistogram(descriptor.RowHistogram, new double[]{0.3, 0.7});
            checkHistogram(descriptor.ColumnHistogram, new double[]{0.1, 0.5, 0.4});
        }

        [Fact]
        public void FloatBitmap()
        {
            var bitmap = new float[1,5]{
                {0.5f, 9.0f, 0.0f, 0.01f, 0.49f}
            };
            var descriptor = new BitmapDescriptor<float>(bitmap);
            checkSize<float>(descriptor, 1, 5);
            checkHistogram(descriptor.RowHistogram, new double[]{1.0});
            checkHistogram(descriptor.ColumnHistogram, new double[]{0.05, 0.9, 0.0, 0.001, 0.049});
        }

        [Fact]
        public void DoubleBitmap()
        {
            var bitmap = new double[4,1]{
                {0.9}, {8.0}, {1.0}, {0.1}
            };
            var descriptor = new BitmapDescriptor<double>(bitmap);
            checkSize<double>(descriptor, 4, 1);
            checkHistogram(descriptor.RowHistogram, new double[]{0.09, 0.8, 0.1, 0.01});
            checkHistogram(descriptor.ColumnHistogram, new double[]{1.0});
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
