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
        public int[] RowWindows
        {
            get;
            private set;
        }
        public int[] ColumnWindows
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

        public void ApplyFiltering(double lowpass)
        {
            if (nColumns > 1)
            {
                ColumnWindows = GetSortedFrequencies(ColumnHistogram, lowpass);
                //FilterByMovingAverage(ColumnHistogram, ColumnWindows[0]);
            }
            if (nRows > 1)
            {
                RowWindows = GetSortedFrequencies(RowHistogram, lowpass);
            }
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
            // creating a vector of complex samples
            int lpf = (int)(data.Length * lowpass);
            Complex32[] samples = new Complex32[data.Length];
            for (int ii = 0; ii < data.Length; ii++)
            {
                samples[ii] = new Complex32((float)data[ii], 0.0f);
            }
            // do the FFT
            Fourier.Forward(samples, FourierOptions.NoScaling);
            // get the first half of the frequencies
            // - removing the very first one
            // - removing some lowpass values
            int Ncorr = lpf + 1;
            int Nf = data.Length/2 - Ncorr;
            double[] freq = new Double[Nf];
            for (int ii = 0; ii < Nf; ii++)
            {
                freq[ii] = samples[ii+Ncorr].Magnitude;
            }
            // sort along the signal strength
            var sorted = freq
                .Select((x, i) => new KeyValuePair<int, double>(i, x))
                .OrderByDescending(x => x.Value)
                .ToList();
            // add 1 again to correct for removing first frequency
            return sorted.Select(x => x.Key + Ncorr).ToArray();
        }

        private void FilterByMovingAverage(double[] data, int window)
        {
            var ms = new MovingStatistics(window, data);
        }
    }
}
