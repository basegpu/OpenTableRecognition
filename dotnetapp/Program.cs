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
                var bitmap = reader.GetBinaryBitmap(0.5f, 0.2);
                var descriptor = new BitmapDescriptor<bool>(bitmap);
                using (StreamWriter file = new StreamWriter("rows.dat"))
                {
                    foreach (var v in descriptor.RowHistogram)
                    {
                        file.Write(v);
                        file.Write(Environment.NewLine);
                    }
                }
                using (StreamWriter file = new StreamWriter("cols.dat"))
                {
                    foreach (var v in descriptor.ColumnHistogram)
                    {
                        file.Write(v);
                        file.Write(Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
