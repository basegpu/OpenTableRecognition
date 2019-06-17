using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpenTableRecognition
{
    public class ImageReader : IDisposable
    {
        private readonly string _path;

        public ImageReader(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"File {path} does not exist.");
            }
            _path = path;
        }

        public void Dispose()
        {
            
        }

        public Image<Rgba32> GetImage()
        {
            return Image.Load(_path);
        }

        public Image<Rgba32> GetImageGrayscale()
        {
            var img = GetImage();
            img.Mutate(i => i.Grayscale());
            return img;
        }

        public Image<Rgba32> GetImageBlackWhite()
        {
            var img = GetImage();
            img.Mutate(i => i.BlackWhite());
            return img;
        }

        public Image<Rgba32> GetImageBlackWhite(float treshold)
        {
            var img = GetImage();
            img.Mutate(i => i.BlackWhite().BinaryThreshold(treshold));
            return img;
        }

        public bool[,] GetBinaryBitmap(float threshold, double scale=1.0)
        {
            var image = GetImageBlackWhite(threshold);
            if (scale != 1.0)
            {
                int width = (int)(image.Width * scale);
                int height = (int)(image.Height * scale);
                image.Mutate(i => i.Resize(width, height, true));
            }
            var bitmap = new bool[image.Height, image.Width];
            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    bitmap[h, w] = image[w, h].R < 128 ? true : false;
                }
            }
            return bitmap;
        }
    }
}
