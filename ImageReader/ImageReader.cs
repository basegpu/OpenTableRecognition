using System;
using System.Drawing;

namespace OpenTableRegonition
{
    public class ImageReader : IDisposable
    {
        private readonly string _path;

        public ImageReader(string path)
        {
            _path = path;
        }


        public void Dispose()
        {
            
        }

        public Bitmap GetImage()
        {
            var img = Bitmap.FromFile(_path);
            return new Bitmap(img);
        }
    }
}
