using System;
using System.Collections.Generic;

namespace VectorMath
{
    public static class EarClippingTriangulator
    {
        public static int[] GetBaseTries(Vector2[] vertices)
        {
            int count = vertices.Length;

            List<int> indexes = GetIndexes(count);

            List<int> tries = new List<int>((count - 2) * 6 + count * 6);

            CalculateTriangle(vertices, indexes, tries);
            return tries.ToArray();
        }

        public static int[] GetTriangles(Vector2[] vertices)
        {
            int count = vertices.Length;

            List<int> indexes = GetIndexes(count);

            List<int> tries = new List<int>((count - 2) * 6 + count * 6);

            CalculateTriangle(vertices, indexes, tries);

            int baseTriCount = (count - 2) * 3;

            // Middle loop Faces
            for (int i = 1, t = baseTriCount; i < count * 2; i += 2, t += 6)
            {
                tries.Add(count + i);
                tries.Add(3 * count + i);
                tries.Add(count + (i + 1) % (count * 2));
                tries.Add(count + (i + 1) % (count * 2));
                tries.Add(3 * count + i);
                tries.Add(3 * count + (i + 1) % (count * 2));
            }

            int start = count * 5;
            for (int i = 0; i < baseTriCount; i += 3)
            {
                tries.Add(tries[i] + start);
                tries.Add(tries[i + 2] + start);
                tries.Add(tries[i + 1] + start);
            }

            return tries.ToArray();
        }

        private static void CalculateTriangle(Vector2[] vertices, List<int> indexes, List<int> triangles, bool clockwise = false)
        {
            if (indexes.Count == 3)
            {
                if (clockwise)
                {
                    triangles.Add(indexes[0]);
                    triangles.Add(indexes[2]);
                    triangles.Add(indexes[1]);
                }
                else
                {
                    triangles.Add(indexes[0]);
                    triangles.Add(indexes[1]);
                    triangles.Add(indexes[2]);
                }

                indexes.Clear();
                return;
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                double angle = GetAngle(vertices, indexes, i);
                bool inside = IsInside(vertices, indexes, i);
                if (angle >= Math.Abs(180f) || inside)
                {
                    continue;
                }

                if (clockwise)
                {
                    triangles.Add(indexes[i]);
                    triangles.Add(indexes[ClampIndex(i - 1, indexes.Count)]);
                    triangles.Add(indexes[ClampIndex(i + 1, indexes.Count)]);
                }
                else
                {
                    triangles.Add(indexes[i]);
                    triangles.Add(indexes[ClampIndex(i + 1, indexes.Count)]);
                    triangles.Add(indexes[ClampIndex(i - 1, indexes.Count)]);
                }

                indexes.RemoveAt(i);
                CalculateTriangle(vertices, indexes, triangles, clockwise);
                return;
            }
        }

        private static double GetAngle(Vector2[] vertices, List<int> indexes, int index)
        {
            var dir = (vertices[indexes[index]] - vertices[indexes[ClampIndex(index - 1, indexes.Count)]]).Normalized;
            var vector = (vertices[indexes[index]] - vertices[indexes[ClampIndex(index + 1, indexes.Count)]]).Normalized;

            return (Vector2.Angle(dir, vector) + 360) % 360;
        }

        private static bool IsInside(Vector2[] vertices, List<int> indexes, int currentIndex)
        {
            for (int index = 0; index < indexes.Count - 3; index++)
            {
                int i = ClampIndex(index + currentIndex + 2, indexes.Count);
                int orientation1 = GetOrientation(vertices[indexes[ClampIndex(currentIndex - 1, indexes.Count)]], vertices[indexes[currentIndex]], vertices[indexes[i]]);
                int orientation2 = GetOrientation(vertices[indexes[currentIndex]], vertices[indexes[ClampIndex(currentIndex + 1, indexes.Count)]], vertices[indexes[i]]);
                int orientation3 = GetOrientation(vertices[indexes[ClampIndex(currentIndex + 1, indexes.Count)]], vertices[indexes[ClampIndex(currentIndex - 1, indexes.Count)]], vertices[indexes[i]]);

                if (orientation1 == 0 || orientation2 == 0 || orientation3 == 0 || (orientation1 == orientation2 && orientation2 == orientation3))
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetOrientation(Vector2 p1, Vector2 p2, Vector2 point)
        {
            var value = (point.Y - p1.Y) * (p2.X - p1.X) - (point.X - p1.X) * (p2.Y - p1.Y);
            if (value == 0)
                return 0;

            return value < 0 ? 1 : 2;
        }

        private static List<int> GetIndexes(int count)
        {
            List<int> indexes = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                indexes.Add(i);
            }

            return indexes;
        }

        private static int ClampIndex(int index, int count)
        {
            index %= count;
            if (index < 0)
            {
                index = count + index;
            }

            return index;
        }
    }
}
