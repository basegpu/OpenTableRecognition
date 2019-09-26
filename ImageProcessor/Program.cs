
using OpenCvSharp;
using OpenTableRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TableExtractor;

namespace ImageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {

            
            var image = new ImageReader("images/receipt.jpg");
            var cd = new ContourDistanceApproach(image.GetImageMat());
            var contours = cd.GetLetterBoundingBoxes(4);

            DrawBoxesAndSave(image.GetImageMat(), contours, "images/outRects.jpg");
            
            var clusters = cd.GetClustersByLine(contours).ToArray();
            DrawBoxesAndSave(image.GetImageMat(), clusters.SelectMany(c=>c).ToArray(), "images/outClusters.jpg");
        

            //OCR
            //var imgReader = new ImageReader("images/receiptDilated.png");
            //var img = ImageReader.Encode(imgReader.GetImageBlackWhite());

            //var bytes = new List<byte>();
            //byte[] buffer = new byte[1024 * 64];

            //int read;
            //while ((read = img.Read(buffer, 0, buffer.Length)) > 0)
            //{

            //    bytes.AddRange(buffer);
            //}

            //var text = OcrProcessor.GetText(bytes.ToArray(), "eng+deu");
            //using (var f = new StreamWriter("images/ocrResult.txt"))
            //{
            //    f.WriteLine(text);
            //}
        }


        private static void DrawBoxesAndSave(Mat src, Rect[] boundingBox, string output)
        {
            using (var dest = new Mat(src.Cols, src.Rows, MatType.CV_16UC3))
            {
                Cv2.CvtColor(src, dest, ColorConversionCodes.GRAY2BGR);
                
                for (int i = 0; i < boundingBox.Length; i++)
                {
                    Cv2.Rectangle(dest, boundingBox[i], new Scalar(0, 255, 0));
                }

                dest.SaveImage(output);
            }
        }
    }
}
