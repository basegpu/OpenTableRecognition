using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Extreme.Statistics;

namespace OpenTableRecognition
{
    public class BitmapDescriptor<T>
    {

        private readonly T[,] _img;
        public readonly int nRows;
        public readonly int nColumns;
        public double[] RowHistogram
        {
            get;
            private set;
        }
        public double[] ColumnHistogram
        {
            get;
            private set;
        }

        public BitmapDescriptor(T[,] bitmap)
        {
            _img = bitmap;
            nRows = _img.GetLength(0);
            nColumns = _img.GetLength(1);
            CalcHistogrmas();
        }

        private void CalcHistogrmas()
        {
            RowHistogram = new double[nRows];
            ColumnHistogram = new double[nColumns];
            double sum = 0.0;
            double v;
            for (int ri = 0; ri < nRows; ri++)
            {
                for (int ci = 0; ci < nColumns; ci++)
                {
                    v = Convert.ToDouble(_img[ri, ci]);
                    sum += v;
                    RowHistogram[ri] += v;
                    ColumnHistogram[ci] += v;
                }
            }
            double invSum = 1.0 / sum;
            for (int ri = 0; ri < nRows; ri++)
            {
                RowHistogram[ri] *= invSum;
            }
            for (int ci = 0; ci < nColumns; ci++)
            {
                ColumnHistogram[ci] *= invSum;
            }
            // var rows = KernelDensity.Estimate(ColumnHistogram, KernelDensity.GaussianKernel);
            // foreach (var item in rows)
            // {
            //     Console.WriteLine(item);
            // }
        }
    }
}
