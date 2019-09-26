using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class TableExtractorUtilsTests
    {

        [Theory]
        [InlineData(1, 1, 2, 2, 5, 0)]
        [InlineData(1, 1, 6, 1, 5, 1)]
        [InlineData(1, 1, 10, 1, 5, 5)]
        public void HorizontalDistance(int x1, int y1, int x2, int y2, int size, int expectedDistance)
        {
            var rect1 = new Rect(x1, y1, size, size);
            var rect2 = new Rect(x2, y2, size, size);
            var distances = TableExtractor.Utils.HorizontalDistances(new Rect[] { rect1, rect2 });
            Assert.Single(distances);
            Assert.Equal(expectedDistance, distances.First());
        }

        [Theory]
        [InlineData(2, 2, 1, 1, 5, 0)]
        [InlineData(6, 1, 1, 1, 5, 1)]
        [InlineData(10, 1, 1, 1, 5, 5)]
        public void SortedHorizontalDistance(int x1, int y1, int x2, int y2, int size, int expectedDistance)
        {
            var rect1 = new Rect(x1, y1, size, size);
            var rect2 = new Rect(x2, y2, size, size);
            var distances = TableExtractor.Utils.HorizontalDistancesSorted(new Rect[] { rect1, rect2 });
            Assert.Single(distances);
            Assert.Equal(expectedDistance, distances.First());
        }

        [Theory]
        [MemberData(nameof(RectData))]
        public void GroupIntoLinesUniformElements(Rect[] elements, int expectedLineCount)
        {
            Assert.Equal(expectedLineCount, TableExtractor.Utils.GroupBoxesInLines(elements).Count());
        }

        public static IEnumerable<object[]> RectData =>
        new List<object[]>
        {
                new object[] { new Rect[] { new Rect(1, 1, 5, 5), new Rect(6, 1, 10, 5) }, 1 },
                new object[] { new Rect[] { new Rect(1, 1, 5, 5), new Rect(1, 3, 5, 5) }, 1 }, //overlapping boundaries
                new object[] { new Rect[] { new Rect(1, 1, 5, 5), new Rect(2, 2, 2, 2) }, 1 }, //one inside another
                new object[] { new Rect[] { new Rect(1, 5, 5, 5), new Rect(1, 1, 5, 5) }, 1 }, //on the edge
                new object[] { new Rect[] { new Rect(1, 6, 5, 5), new Rect(1, 1, 5, 5) }, 2 }, //just below
                new object[] { new Rect[] { new Rect(1, 100, 5, 5), new Rect(1, 1, 5, 5) }, 2 },
                new object[] { new Rect[] { new Rect(1, 100, 5, 5), new Rect(1, 6, 5, 5), new Rect(1, 1, 5, 5) }, 3 },
                new object[] { new Rect[] { new Rect(1, 1, 5, 5), new Rect(1, 100, 5, 5), new Rect(6, 1, 10, 5) }, 2 } //not ordered
        };

        [Fact]
        public void HistogramEven()
        {
            var samples = new int[] { 2, 1, 2, 0, 1, 0 };
            var hist = TableExtractor.Utils.Histogram(samples, 3);
            Assert.True(hist.Count() == 3);
            Assert.True(hist.All(h => h.value == 2.0m / 6.0m));
        }

        [Fact]
        public void HistogramSparse()
        {
            var samples = new int[] { 3, 2, 2, 3, 3, 3, 96, 3, 3, 2, 3, 4, 0, 2, 5, 6, 5, 4, 3, 2, 99, 2, 3, 4, 5 };
            var hist = TableExtractor.Utils.Histogram(samples, 10);
            Assert.True(hist.Count() == 10);
            Assert.True(hist.First().value == 23.0m / 25.0m);
            Assert.True(hist[9].value == 2.0m / 25.0m);
        }

        [Fact]
        public void Maxima()
        {
            var samples = new decimal[] { 1, 1, 8, 8, 5, 4, 1, 1, 2, 2, 3, 1, 5, 6, 7, 8 };
            var maxima = TableExtractor.Utils.Maxima(samples);
            Assert.True(maxima.Count() == 3);
            Assert.Equal(new int[] { 3, 10, 15 }, maxima);
        }
    }
}
