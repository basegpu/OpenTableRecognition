using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.Statistics;

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
            CalcHistograms();
        }

        public void ApplyFiltering()
        {
            var ic = GetSortedFrequencies(ColumnHistogram, 0.02);
            FilterByMovingAverage(ic[0]);
            GetSortedFrequencies(RowHistogram, 0.02);
        }

        private void CalcHistograms()
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
        }

        private int[] GetSortedFrequencies(double[] data, double lowpass)
        {
            int lpf = (int)(data.Length * lowpass);
            Complex32[] samples = new Complex32[data.Length];
            for (int ii = 0; ii < data.Length; ii++)
            {
                samples[ii] = new Complex32((float)data[ii], 0.0f);
            }
            Fourier.Forward(samples, FourierOptions.NoScaling);
            int Nf = data.Length/2 - lpf;
            double[] freq = new Double[Nf];
            for (int ii = 0; ii < Nf; ii++)
            {
                freq[ii] = samples[lpf+ii].Magnitude;
            }
            var sorted = freq
                .Select((x, i) => new KeyValuePair<int, double>(i, x))
                .OrderByDescending(x => x.Value)
                .ToList();
            freq = sorted.Select(x => x.Value).ToArray();
            var idx = sorted.Select(x => x.Key + lpf).ToArray();
            return idx;
        }

        private void FilterByMovingAverage(int window)
        {

        }
    }
}
