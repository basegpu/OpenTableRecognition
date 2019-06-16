using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTableRecognition
{
    public class BitmapDescriptor<T>
    {

        private readonly T[,] _img;
        public float[] RowHistogram
        {
            get;
            private set;
        }
        public float[] ColumnHistogram
        {
            get;
            private set;
        }

        public BitmapDescriptor(T[,] bitmap)
        {
            _img = bitmap;
            CalcHistogrmas();
        }

        private void CalcHistogrmas()
        {
            RowHistogram = new float[_img.GetLength(0)];
            ColumnHistogram = new float[_img.GetLength(1)];
        }
    }
}
