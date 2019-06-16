using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenTableRecognition
{
    public class BitmapDescriptor
    {

        private readonly Bitmap _img;
        private short[] _rowHistogram = default(short[]);
        private short[] _columnHistogram = default(short[]);

        public BitmapDescriptor(Bitmap bitmap)
        {
            _img = bitmap;
        }

        public short[] GetRowHistogram()
        {
            return _rowHistogram;
        }

        public short[] GetColumnHistogram()
        {
            return _columnHistogram;
        }

    }
}
