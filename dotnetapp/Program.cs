using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using OpenTableRecognition;
using static System.Console;

public static class Program
{
    public static void Main(string[] args) 
    {
        if (args.Length == 1)
        {
            try
            {
                var reader = new ImageReader(args[0]);
                var bitmap = reader.GetBinaryBitmap(0.5f);
                var descriptor = new BitmapDescriptor<bool>(bitmap);
                foreach (var v in descriptor.ColumnHistogram)
                {
                    Console.WriteLine($"{v}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
