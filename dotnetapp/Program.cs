using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using OpenTableRegonition;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
