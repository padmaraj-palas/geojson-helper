using System;
using System.Collections.Generic;

namespace VectorMath
{
    public struct PolyBound
    {
        public List<Vector2> Points;

        private Vector2 _center;
        private double _innerRadius;
        private Vector2 _max;
        private Vector2 _min;
        private double _outerRadius;
        private bool _dataUpdated;

        public Vector2 Center
        {
            readonly get
            {
                return _center;
            }

            private set
            {
                _center = value;
            }
        }

        public double InnerRadius
        {
            readonly get
            {
                return _innerRadius;
            }

            private set
            {
                _innerRadius = value;
            }
        }

        public Vector2 Max
        {
            readonly get
            {
                return _max;
            }

            private set
            {
                _max = value;
            }
        }

        public Vector2 Min
        {
            readonly get
            {
                return _min;
            }

            private set
            {
                _min = value;
            }
        }

        public double OuterRadius
        {
            readonly get
            {
                return _outerRadius;
            }

            private set
            {
                _outerRadius = value;
            }
        }

        public void CalculateBounds()
        {
            if (Points.Count == 0 || _dataUpdated)
            {
                return;
            }

            Max = new Vector2(float.MinValue, float.MinValue);
            Min = new Vector2(float.MaxValue, float.MaxValue);
            Center = Vector2.Zero;
            InnerRadius = float.MaxValue;
            OuterRadius = float.MinValue;
            foreach (var point in Points)
            {
                Center += point;
                Max = new Vector2(Math.Max(Max.X, point.X), Math.Max(Max.Y, point.Y));
                Min = new Vector2(Math.Min(Min.X, point.X), Math.Min(Min.Y, point.Y));
            }

            Center = Center / Points.Count;

            for (int i = 0; i < Points.Count; i++)
            {
                double distance = Vector2.Distance(Points[i], Center);
                Line line = new Line { Start = Points[i], End = Points[Points.LoopIndex(i + 1)] };
                if (line.TryProjectPointOnLine(Center, out Vector2 point))
                {
                    InnerRadius = Math.Min(InnerRadius, Vector2.Distance(point, Center));
                }

                InnerRadius = Math.Min(InnerRadius, distance);
                OuterRadius = Math.Max(OuterRadius, distance);
            }

            _dataUpdated = true;
        }

        public bool IsPointInside(Vector2 position)
        {
            var center = Center;
            var distance = Vector2.Distance(position, center);

            if (distance <= InnerRadius)
            {
                return true;
            }

            if (distance > OuterRadius)
            {
                return false;
            }

            int count = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                var startIndex = i;
                var endIndex = Points.LoopIndex(i + 1);
                Line line1 = new Line { Start = Points[startIndex], End = Points[endIndex] };
                Line line2 = new Line { Start = position, End = center };

                if (Line.TryGetLineLineIntersection(line1, line2, out Vector2 point))
                {
                    count++;
                }
            }

            return count % 2 == 0;
        }

        public void SetPoints(List<Vector2> points)
        {
            _dataUpdated = false;
            Points = points;
            CalculateBounds();
        }

        public bool TryGetPointOnEdge(Vector2 position, out Line.LineIntersectionData intersectionData)
        {
            double distance = float.MaxValue;
            intersectionData = default;
            for (int i = 0; i < Points.Count; i++)
            {
                var startIndex = i;
                var endIndex = Points.LoopIndex(i + 1);
                Line line = new Line { Start = Points[startIndex], End = Points[endIndex] };
                if (line.TryProjectPointOnLine(position, out Vector2 point))
                {
                    double dist = Vector2.Distance(point, position);
                    if (dist <= InnerRadius * 0.25f && dist < distance)
                    {
                        distance = dist;
                        intersectionData = new Line.LineIntersectionData
                        {
                            Line = line,
                            LineIndex = i,
                            PointOnLine = point
                        };
                    }
                }
            }

            return intersectionData != default;
        }
    }
}
