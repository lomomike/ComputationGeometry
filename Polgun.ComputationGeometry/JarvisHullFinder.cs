using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Polgun.ComputationGeometry
{
    internal class JarvisHullFinder
    {
        private readonly List<Point> _points;

        public JarvisHullFinder(IEnumerable<Point> points)
        {
            Contract.Requires(points != null);

            _points = new List<Point>(points);
            if(_points.Count <=1)
                throw new ArgumentOutOfRangeException("points");
        }
        
        public IEnumerable<Point> Find()
        {
            var result = new List<Point>();

            Point leftDownPoint = FindLeftDownPoint();

            _points.Remove(leftDownPoint);
            result.Add(leftDownPoint);

            FormHull(leftDownPoint, result);

            return result;
        }

        private Point FindLeftDownPoint()
        {
            Point leftDownPoint = _points[0];
            for (int index = 1; index < _points.Count; ++index)
            {
                var currentPoint = _points[index];
                if (currentPoint.X < leftDownPoint.X)
                {
                    leftDownPoint = currentPoint;
                }
                else if (Math.Abs(currentPoint.X - leftDownPoint.X) < Point.Epsilon &&
                         currentPoint.Y > leftDownPoint.Y)
                {
                    leftDownPoint = currentPoint;
                }
            }

            return leftDownPoint;

        }

        private void FormHull(Point leftDownPoint, List<Point> result)
        {
            Point currentPoint = leftDownPoint;

            Point nextPoint = Point.Empty;
            int nextIt = -1;

            do
            {
                //if currentPoint == leftDownPoint, than the next for loop will change value of nextPoint 
                //if the next for loop doesn't change - polygon is closed, stop looping
                if (RightTurn(nextPoint, currentPoint, leftDownPoint))
                {
                    nextPoint = leftDownPoint;
                    nextIt = _points.Count;
                }

                for (int index = 0; index != _points.Count; index++)
                {
                    Point tryPoint = _points[index];
                    
                    if (RightTurn(nextPoint, currentPoint, tryPoint))
                    {
                        nextPoint = tryPoint;
                        nextIt = index;
                    }
                }

                if (nextIt >= 0 && nextIt < _points.Count)
                {
                    _points.RemoveAt(nextIt);
                    result.Add(nextPoint);
                    currentPoint = nextPoint;
                }
            } while (nextPoint != leftDownPoint);
        }

        private bool RightTurn(Point a, Point b, Point c)
        {
            return (((a.X - b.X) * (c.Y - b.Y) - (c.X - b.X) * (a.Y - b.Y)) >= 0);
        }
    }
}