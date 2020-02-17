using System;
using System.Collections.Generic;

namespace MatcherChief.Shared
{
    public static class ArraySegmentHelper
    {
        public static List<ArraySegment<T>> Segment<T>(T[] items, int itemsPerSegment)
        {
            var segments = new List<ArraySegment<T>>();

            var i = 0;
            var offset = 0;
            var count = itemsPerSegment;
            while (offset < items.Length)
            {
                if (offset + count > items.Length)
                    count = items.Length - offset;

                segments.Add(new ArraySegment<T>(items, offset, count));

                i++;
                offset = i * itemsPerSegment;
                count = itemsPerSegment;
            }

            return segments;
        }
    }
}