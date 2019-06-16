using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        public Image<Rgba32> GetImage()
        {
            return Image.Load(_path);
        }

        public Image<Rgba32> GetImageGrayscale()
        {
            var img = GetImage();
            return img;
        }

        public bool[,] GetImageBinary()
        {
            return GetGrayscaleImageBinary(GetImageGrayscale());
        }

        /// <summary>
        /// First dimension is height, second is width
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool[,] GetGrayscaleImageBinary(Image<Rgba32> image)
        {
            var result = new bool[image.Height, image.Width];
            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                  //  image.GetPixel(h, w);
                    
                }
            }
            return result;
        }
    }
}
