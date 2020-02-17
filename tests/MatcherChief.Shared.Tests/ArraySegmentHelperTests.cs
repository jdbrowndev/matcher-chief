using System.Linq;
using Xunit;

namespace MatcherChief.Shared.Tests
{
    public class ArraySegmentHelperTests
    {
        [Theory]
        [InlineData(12, 2, 6)]
        [InlineData(11, 2, 6)]
        [InlineData(12, 1024, 1)]
        public void Should_Properly_Segment_Bytes(int numberOfBytes, int bytesPerSegment, int expectedSegments)
        {
            var bytes = new byte[numberOfBytes];
            var segments = ArraySegmentHelper.Segment(bytes, bytesPerSegment);
            Assert.Equal(expectedSegments, segments.Count);
        }

        [Fact]
        public void Should_Handle_No_Segments()
        {
            var segments = ArraySegmentHelper.Segment(new byte[0], 1024);
            Assert.Empty(segments);
        }

        [Fact]
        public void Should_Properly_Index_Single_Segment()
        {
            var bytes = new byte[568];
            var segments = ArraySegmentHelper.Segment(bytes, 1024);

            Assert.Single(segments);
            var segment = segments.Single();
            Assert.Equal(0, segment.Offset);
            Assert.Equal(568, segment.Count);
        }

        [Fact]
        public void Should_Properly_Index_Multiple_Segments()
        {
            var bytes = new byte[202];
            var segments = ArraySegmentHelper.Segment(bytes, 100);

            Assert.Equal(3, segments.Count);
            
            var segment1 = segments[0];
            Assert.Equal(0, segment1.Offset);
            Assert.Equal(100, segment1.Count);

            var segment2 = segments[1];
            Assert.Equal(100, segment2.Offset);
            Assert.Equal(100, segment2.Count);

            var segment3 = segments[2];
            Assert.Equal(200, segment3.Offset);
            Assert.Equal(2, segment3.Count);
        }
    }
}
