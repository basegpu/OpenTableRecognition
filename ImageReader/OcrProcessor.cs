using BitMiracle.LibTiff.Classic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenTableRecognition
{
    public class OcrProcessor
    {

        public static string GetText(byte[] tifImage, string lang)
        {
            string content = string.Empty;
            var pix = Tesseract.Pix.LoadTiffFromMemory(tifImage);
            using (var engine = new Tesseract.TesseractEngine("tessdata",  lang))
            {
                engine.SetVariable("load_system_dawg", "false");
                engine.SetVariable("load_freq_dawg", "false");
                var page = engine.Process(pix, Tesseract.PageSegMode.SingleBlock);
                
                content = page.GetText();
            }
            return content;
        }

        public static string GetTextIron(string path)
        {
            string content = string.Empty;
            var ocr = new IronOcr.AdvancedOcr();
            var results = ocr.ReadMultiFrameTiff(path);
            return results.Text;
            
        }
    }
}
