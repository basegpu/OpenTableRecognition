using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableExtractor
{
    public class Utils
    {


        public static IEnumerable<Rect[]> GroupBoxesInLines(Rect[] boundingBoxes)
        {
            int i = 0;
            var lines = new List<Rect[]>();
            var sortedContours = boundingBoxes.OrderBy(x => x.Top).ToList();

            while (i < boundingBoxes.Length)
            {
                var lineContours = sortedContours.Skip(i);
                var first = lineContours.FirstOrDefault();
                if (first != null)
                {
                    lineContours = lineContours.TakeWhile(c => IsSameLine(first, c)).ToArray();
                    lines.Add(lineContours.ToArray());
                    i += lineContours.Count();
                }
                else
                {
                    lines.Add(new Rect[] { sortedContours[i] });
                    i++;
                }
            }
            return lines;
        }

        
        internal static bool IsSameLine(Rect a, Rect b)
        {
            return a.Top <= b.Bottom && a.Bottom >= b.Top;
        }

        public static int[] HorizontalDistancesSorted(Rect[] boundingBoxes)
        {
            return HorizontalDistances(boundingBoxes.OrderBy(w => w.Left).ToArray());
        }

        public static int[] HorizontalDistances(Rect[] boundingBoxes)
        {
            var distances = new int[boundingBoxes.Length - 1];
            
            for (int i = 0; i < boundingBoxes.Length - 1; i++)
            {
                distances[i] = HorizontalDistance(boundingBoxes[i], boundingBoxes[i + 1]);
            }
            return distances;
        }

        internal static int HorizontalDistance(Rect a, Rect b)
        {
            if (a.Right > b.Left)
                return 0;
            return b.Left - a.Right;
        }

        public static (int rangeStart, int rangeEnd, decimal value)[] Histogram(IEnumerable<int> samples, int bins = 30)
        {
            var count = samples.Count();
            var result = new (int, int, decimal)[bins];
            var zero = samples.Min();
            int range = (int)Math.Ceiling((decimal)(samples.Max() + 1 - zero) / bins);
            var start = zero; 
            var end = start + range;
            for (int i = 0; i < bins; i++)
            {                
                var elements = samples.Where(d => d >= start && d < end).Count();
                result[i] = (start, end, (decimal)elements / count);
                start = end;
                end = end + range;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns>positions of maxima in the given histogram</returns>
        public static int[] Maxima(decimal[] values, decimal threshold = 0)
        {
            var maxima = new List<int>();
            var prev = decimal.MinValue;
            bool descending = false;
            for (int i = 0; i < values.Length; i++)
            {
                if(values[i] > prev)
                {
                    descending = false;
                }
                else if(!descending && values[i] < prev - threshold)
                {
                    maxima.Add(i-1);
                    descending = true;
                }
                prev = values[i];
            }
            if (!descending)
                maxima.Add(values.Length - 1);
            return maxima.ToArray();
        }
    }
}
