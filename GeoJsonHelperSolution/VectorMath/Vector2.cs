using System;

namespace VectorMath
{
    public struct Vector2
    {
        public double X;
        public double Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public readonly double Magnitude => Math.Sqrt(SqrMagnitude);
        public readonly Vector2 Normalized => Magnitude > 0 ? this / Magnitude : Zero;
        public readonly double SqrMagnitude => X * X + Y * Y;

        public override readonly bool Equals(object? obj)
        {
            return obj is Vector2 vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Vector2 Down => new Vector2(0, -1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Zero => new Vector2(0, 0);

        public static double Angle(Vector2 v1, Vector2 v2)
        {
            return Math.Atan2(v2.Y * v1.X - v2.X * v1.Y, v2.X * v1.X + v2.Y * v1.Y) * 180.0 / Math.PI;
        }

        public static double Distance(Vector2 v1, Vector2 v2)
        {
            return (v2 - v1).Magnitude;
        }

        public static double Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Vector2 operator *(Vector2 vector, double scalar)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2 operator *(double scalar, Vector2 vector)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2 operator /(Vector2 vector, double scalar)
        {
            return new Vector2(vector.X / scalar, vector.Y / scalar);
        }
    }
}
