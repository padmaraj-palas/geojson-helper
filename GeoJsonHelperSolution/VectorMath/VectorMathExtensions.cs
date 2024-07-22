using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMath
{
    public static class VectorMathExtensions
    {
        public static int LoopIndex<T>(this T[] array, int index)
        {
            if (array == null || array.Length == 0)
            {
                return 0;
            }

            return LoopIndex(index, array.Length);
        }

        public static int LoopIndex<T>(this IList<T> list, int index)
        {
            if (list == null || list.Count == 0)
            {
                return 0;
            }

            return LoopIndex(index, list.Count);
        }

        public static int LoopIndex(int index, int max)
        {
            if (index < 0)
            {
                return max + index;
            }

            return index % max;
        }
    }
}
