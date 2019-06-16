using System;
using System.Drawing;
using System.Drawing.Imaging;

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

        public Bitmap GetImageGrayscale()
        {
            //https://web.archive.org/web/20110827032809/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
            //create a blank bitmap the same size as original
            var img = Bitmap.FromFile(_path);
            Bitmap newBitmap = new Bitmap(img.Width, img.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
               0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
