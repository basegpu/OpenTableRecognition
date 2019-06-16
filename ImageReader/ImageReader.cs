﻿using System;
using System.IO;
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
            img.Mutate(x => x.Grayscale());
            return img;
        }

        public Image<Rgba32> GetImageBlackWhite()
        {
            var img = GetImage();
            img.Mutate(x => x.BlackWhite());
            return img;
        }

        public Image<Rgba32> GetImageBlackWhite(float treshold)
        {
            var img = GetImage();
            img.Mutate(x => x.BlackWhite().BinaryThreshold(treshold));
            return img;
        }

        //public bool[,] GetImageBinary(flo )
        //{
        //    return GetImageBinary(GetImageGrayscale());
        //}

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
