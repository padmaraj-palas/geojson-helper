using System;

namespace VectorMath
{
    public struct Line
    {
        public Vector2 End;
        public Vector2 Start;

        public readonly Vector2 Difference => End - Start;
        public readonly double Length => Vector2.Distance(Start, End);

        public static bool operator ==(Line line1, Line line2)
        {
            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
        {
            return !line1.Equals(line2);
        }

        public static bool TryGetLineLineIntersection(Line line1, Line line2, out Vector2 point)
        {
            point = Vector2.Zero;

            Vector2 p1 = line1.Start;
            Vector2 p2 = line1.End;
            Vector2 p3 = line2.Start;
            Vector2 p4 = line2.End;
            Vector2 p13 = p1 - p3;
            Vector2 p43 = p4 - p3;

            if (p43.Magnitude <= float.Epsilon)
            {
                return false;
            }

            Vector2 p21 = p2 - p1;
            if (p21.Magnitude <= float.Epsilon)
            {
                return false;
            }

            double d1343 = Vector2.Dot(p13, p43);
            double d4321 = Vector2.Dot(p43, p21);
            double d1321 = Vector2.Dot(p13, p21);
            double d4343 = Vector2.Dot(p43, p43);
            double d2121 = Vector2.Dot(p21, p21);

            double denom = d2121 * d4343 - d4321 * d4321;
            if (Math.Abs(denom) < float.Epsilon)
            {
                return false;
            }

            double numer = d1343 * d4321 - d1321 * d4343;

            double factorLine1 = numer / denom;

            if (factorLine1 < 0 || factorLine1 > 1)
            {
                return false;
            }

            double factorLine2 = (d1343 + d4321 * factorLine1) / d4343;

            if (factorLine2 < 0 || factorLine2 > 1)
            {
                return false;
            }

            point = p1 + p21 * (float)factorLine1;

            return true;
        }

        public static bool TryProjectPointOnLine(Line line, Vector2 position, out Vector2 point)
        {
            return line.TryProjectPointOnLine(position, out point);
        }

        public readonly bool TryProjectPointOnLine(Vector2 position, out Vector2 point)
        {
            point = Vector2.Zero;
            double numerator = Vector2.Dot(position - Start, Difference);
            double denominator = Vector2.Dot(Difference, Difference);
            if (denominator == 0)
            {
                return false;
            }

            double factor = numerator / denominator;

            if (factor < 0 || factor > 1)
            {
                return false;
            }

            point = Start + Difference * factor;
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Line line &&
                   End.Equals(line.End) &&
                   Start.Equals(line.Start);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(End, Start);
        }

        public struct LineIntersectionData
        {
            public Line Line;
            public int LineIndex;
            public Vector2 PointOnLine;

            public static bool operator ==(LineIntersectionData left, LineIntersectionData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(LineIntersectionData left, LineIntersectionData right)
            {
                return !left.Equals(right);
            }

            public override bool Equals(object obj)
            {
                return obj is LineIntersectionData data &&
                       Line == data.Line &&
                       LineIndex == data.LineIndex &&
                       PointOnLine.Equals(data.PointOnLine);
            }

            public override readonly int GetHashCode()
            {
                return HashCode.Combine(Line, LineIndex, PointOnLine);
            }
        }
    }
}
