using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
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
        public static Rgba32[,] GetColorArray(Image<Rgba32> image)
        {
            var result = new Rgba32[image.Height, image.Width];
            int colNum = 0;
            int rowNum = 0;
            foreach (var pixel in image.GetPixelSpan())
            {
                if (colNum == image.Width)
                {
                    colNum = 0;
                    rowNum++;
                }
                result[rowNum, colNum] = pixel;
                colNum++;
            }
            return result;
        }

        ///// <summary>
        ///// First dimension is height, second is width
        ///// </summary>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //public static bool[,] GetImageBinary(Image<Rgba32> image)
        //{
        //    image.Mutate(x => x.BinaryThreshold(0.3f));
        //    foreach()
        //    var result = new bool[image.Height, image.Width];
        //    for (int h = 0; h < image.Height; h++)
        //    {
        //        for (int w = 0; w < image.Width; w++)
        //        {
        //          //  image.GetPixel(h, w);

        //        }
        //    }
        //    return result;
        //}
    }
}
