using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace TableExtractor
{
    public interface IExtractor
    {

        string[,] GetTableContent();
    }
}
