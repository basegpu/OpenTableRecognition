using System;
using System.IO;
using BitMiracle.LibTiff.Classic;
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

        public static MemoryStream Encode(Image<Rgba32> image)
        {
            var tempFilePath = System.IO.Path.GetTempFileName();
            using (var tif = BitMiracle.LibTiff.Classic.Tiff.Open(tempFilePath, "w"))
            {
                var width = image.Width;
                var height = image.Height;

                tif.SetField(TiffTag.IMAGEWIDTH, width);
                tif.SetField(TiffTag.IMAGELENGTH, height);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);

                tif.SetField(TiffTag.ROWSPERSTRIP, image.Height);

                tif.SetField(TiffTag.XRESOLUTION, image.MetaData.HorizontalResolution);
                tif.SetField(TiffTag.YRESOLUTION, image.MetaData.VerticalResolution);

                tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 4);

                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);

                byte[] color_ptr = new byte[width * 4];
                for (int rows = 0; rows < height; rows++)
                {
                    var pixelRow = image.GetPixelRowSpan(rows);
                    for (var col = 0; col < width; col++)
                    {
                        var pixel = pixelRow[col];
                        
                        color_ptr[col * 4 + 0] = pixel.R;
                        color_ptr[col * 4 + 1] = pixel.G;
                        color_ptr[col * 4 + 2] = pixel.B;
                        color_ptr[col * 4 + 3] = pixel.A;
                    }
                    tif.WriteScanline(color_ptr, rows, 0);
                }

                tif.FlushData();

            }
            var memStream = new MemoryStream(File.ReadAllBytes(tempFilePath));
            File.Delete(tempFilePath);
            return memStream;
        }
    }
}
