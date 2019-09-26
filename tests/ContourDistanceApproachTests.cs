using OpenCvSharp;
using OpenTableRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TableExtractor;
using Xunit;

namespace tests
{
    public class ContourDistanceApproachTests
    {

        [Theory]
        [InlineData("images/cleanBasicTable.jpg")]
        public void GetLetterContours(string path)
        {
            var image = new ImageReader(path);
            var cd = new ContourDistanceApproach(image.GetImageMat());
            var boxes = cd.GetLetterBoundingBoxes(0);

            Assert.Equal(67+5, boxes.Length); //67 characters + 5 dots over 'i' 
        }


        [Theory]
        [InlineData("images/cleanBasicTable.jpg")]
        public void GetClusters(string path)
        {
            var image = new ImageReader(path);
            var cd = new ContourDistanceApproach(image.GetImageMat());
            var boxes = cd.GetLetterBoundingBoxes(0);
            var clusters = cd.GetClustersByLine(boxes).ToArray();

            Assert.Equal(7, clusters.SelectMany(c=>c).Count()); //6 from table, 1 from footer
            
            Assert.Equal(2, clusters[0].Length);
            Assert.Equal(2, clusters[1].Length);
            Assert.Equal(2, clusters[2].Length);
            Assert.Single(clusters[3]);
        }
    }
}
