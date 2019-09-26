using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TableExtractor
{
    public class ContourDistanceApproach : IExtractor
    {
        private readonly Mat _image;
        public ContourDistanceApproach(Mat image)
        {
            _image = image;
        }

        public string[,] GetTableContent()
        {

            return new string[0, 0];
        }


        /// <summary>
        /// We assume we have a white background
        /// </summary>
        /// <param name="dilationElementHeight">Structuring rectangle of this height and width = height/2</param>
        /// <returns></returns>
        public Rect[] GetLetterBoundingBoxes(uint dilationElementHeight = 0)
        {
            using (var src = _image.Clone())
            {
                

                Cv2.BitwiseNot(src, src); //dilation works on black background
                Cv2.Threshold(src, src, 0, 255, ThresholdTypes.Otsu); //binarization, cutting out noise
                if (dilationElementHeight > 1)
                {
                    var structuringElement = OpenCvSharp.Cv2.GetStructuringElement(MorphShapes.Rect, new Size(dilationElementHeight / 2, dilationElementHeight));
                    Cv2.Dilate(src, src, structuringElement, iterations: 1); //dilation to make letters more bold
                }

                var hierarchy = OutputArray.Create(new Mat());
                Cv2.FindContours(src, out var contours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                return contours.Select(c => c.BoundingRect()).ToArray();
            }
        }


        public IEnumerable<Rect[]> GetClustersByLine(Rect[] letterBoundingBoxes)
        {
            var linesOfLetters = Utils.GroupBoxesInLines(letterBoundingBoxes);
            var allDistances = linesOfLetters.SelectMany(lineOfLetters => Utils.HorizontalDistancesSorted(lineOfLetters));

            var histogram = Utils.Histogram(allDistances);
            var maxima = Utils.Maxima(histogram.Select(h=>h.value).ToArray());

            var clusters = new List<Rect[]>();
            if (maxima.Count() >= 2)
            {
                var threshold = histogram[maxima[1]].rangeStart;
                clusters.AddRange(linesOfLetters.Select(lineOfLetters => GroupIntoClusters(lineOfLetters, threshold)).ToArray());
            }
            return clusters;
        }

        internal Rect[] GroupIntoClusters(Rect[] boundingBoxes, int distanceThreshold)
        {
            if (!boundingBoxes.Any())
                return new Rect[0];
            var boxesSorted = boundingBoxes.OrderBy(bb => bb.Left).ToArray();
            var distances = Utils.HorizontalDistances(boxesSorted);
            var result = new List<Rect>();
            var cluster = new List<Rect>() { boxesSorted[0] };
            for (int i = 1; i < boxesSorted.Length; i++)
            {
                if (distances[i - 1] >= distanceThreshold)
                {
                    result.Add(GetBoundingBox(cluster));
                    cluster.Clear();
                }
                cluster.Add(boxesSorted[i]);
            }
            if (cluster.Any())
                result.Add(GetBoundingBox(cluster));

            return result.ToArray();
        }

        //TODO, rather inefficient algorithm
        private Rect GetBoundingBox(List<Rect> boxes)
        {
            var leftX = boxes.Min(b => b.X);
            var leftY = boxes.Min(b => b.Y);
            var rightX = boxes.Max(b => b.Right);
            var rightY = boxes.Max(b=> b.Bottom);
            return new Rect(leftX, leftY, rightX - leftX, rightY - leftY + 1);
        }

        /// <summary>
        /// We calculate the absolute distance between letters in the same line 
        /// </summary>
        internal static IEnumerable<int> GetLetterSpacing(Rect[] letterBoundingBoxes)
        {
            var result = new List<int>();
            foreach(var lineOfBoxes in Utils.GroupBoxesInLines(letterBoundingBoxes).ToArray())
            {
                result.AddRange(Utils.HorizontalDistancesSorted(lineOfBoxes).ToList());
            }
            return result;
        }
        
    }
}
