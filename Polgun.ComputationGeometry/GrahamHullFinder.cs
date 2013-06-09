using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Polgun.ComputationGeometry
{
    internal class GrahamHullFinder
    {
        private List<Point> _points;
        private const int mininumPointsCount = 2;

        public GrahamHullFinder(IEnumerable<Point> points)
        {
            Contract.Requires(points != null);
            
            _points = new List<Point>(points);
            if(_points.Count < mininumPointsCount)
                throw new ArgumentOutOfRangeException("points");
        }

        public IList<Point> Find()
        {
            Point startPoint = GetStartPoint();

            //_points.Sort((a, b) =>
            //{
            //    if (a == b) return 0;
            //    if (Direction(startPoint, b, a) <= 0) return -1;
            //    return 1;
            //});

            _points.Sort((a, b) =>
                {
                    double angle = Math.Atan2(a.Y - startPoint.Y, a.X - startPoint.X) - Math.Atan2(b.Y - startPoint.Y, b.X - startPoint.X);
                    if ((angle < 0) || (angle == 0) && (Hypot(a.Y - startPoint.Y, a.X - startPoint.X) < Hypot(b.Y - startPoint.Y, b.X - startPoint.X)))
                            return -1;
                        return 1;
                });

            List<Point> resultHull = new List<Point>(_points.Count);
            resultHull.Add(_points[0]);
            resultHull.Add(_points[1]);

            for (int i = 2; i < _points.Count; ++i)
            {
                while (resultHull.Count >= 2 && !LeftTurn(resultHull[resultHull.Count - 2], resultHull[resultHull.Count - 1], _points[i]))
                    resultHull.RemoveAt(resultHull.Count - 1);
                resultHull.Add(_points[i]);
            }
            
            return resultHull;
        }

        private Point GetStartPoint()
        {
            const double epsilon = 1e-6;

            double minY = _points.Select(point => point.Y)
                                 .Min();
            double minX = _points.Where(p => Math.Abs(p.Y - minY) < epsilon)
                                 .Select(point => point.X)
                                 .Min();

            return new Point(minX, minY);
        }

        private double Direction(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }

        private bool LeftTurn(Point p1, Point p2, Point p3)
        {
            return Direction(p1, p2, p3) > 0;
        }

        private double Hypot(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }
    }
}