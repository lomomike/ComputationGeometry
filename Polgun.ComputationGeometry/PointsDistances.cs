using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Polgun.ComputationGeometry
{
    public static class PointsDistances
    {
        /// <summary>
        /// Find the closest points on the plane.
        /// </summary>
        /// <param name="points">The sequence of points on a plane.</param>
        /// <returns>The pair of two closest points on the plane and a distance between them.</returns>
        public static FindPairResult FindClosestPair(IList<Point> points)
        {
            return RunPointsCalculation(points, pts =>
            {
                var pnts = new List<Point>(pts);
                pnts.Sort((first, second) => first.X.CompareTo(second.X));

                var ySeries = new List<Point>(pts);
                ySeries.Sort((first, second) => first.Y.CompareTo(second.Y));

                Point point1, point2;
                FindClosestPoints(pnts, ySeries, out point1, out point2);
                return new FindPairResult(point1, point2);
            });
        }

        /// <summary>
        /// Find the fartherst points on the plane.
        /// </summary>
        /// <param name="points">The sequence of points on a plane.</param>
        /// <returns>The pair of two farthest points on the plane and a distance between them.</returns>
        public static FindPairResult FindFarthestPair(IList<Point> points)
        {
            return RunPointsCalculation(points, pts =>
                {
                    Point point1, point2;
                    FindFarthestPoints(pts, out point1, out point2);
                    return new FindPairResult(point1, point2);
                });
        }

        #region Find closest points functions
        private static double FindClosestPoints(List<Point> points, List<Point> ySeries, out Point point1, out Point point2)
        {
            // If count of points is less or equals 3,
            // find the shortest distance with simple search.
            // Calculate the squares of distances to optimize the calculation
            if (points.Count <= 3)
            {
                return SearchClosestInLittleSequrnce(points, out point1, out point2);
            }

            // Find vertical line l, wich devides the set of points to two subsets
            List<Point> leftPoints;
            List<Point> rightPoints;
            List<Point> leftY;
            List<Point> rightY;
            var lIndex = DevidePointsSet(points, ySeries, out leftPoints, out rightPoints, out leftY, out rightY);

            // Make recursive call
            Point ind11, ind12, ind21, ind22;
            double delta1 = FindClosestPoints(leftPoints, leftY, out ind11, out ind12);
            double delta2 = FindClosestPoints(rightPoints, rightY, out ind21, out ind22);

            // Find the least distance in pair
            double delta;
            if (delta1 > delta2)
            {
                delta = delta1;
                point1 = ind11;
                point2 = ind12;
            }
            else
            {
                delta = delta2;
                point1 = ind21;
                point2 = ind22;
            }

            // Find the shortest distance in the 2 * delta field
            return FindInDeltaField(points, ySeries, lIndex, delta, ref point1, ref point2);
        }

        private static double SearchClosestInLittleSequrnce(List<Point> points, out Point point1, out Point point2)
        {
            double minDistance = double.MaxValue;
            point1 = point2 = default(Point);

            for (int i = 0; i < points.Count - 1; ++i)
                for (int j = i + 1; j < points.Count; ++j)
                {
                    double distance = SquareDistance(points[i], points[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        point1 = points[i];
                        point2 = points[j];
                    }
                }

            return minDistance;
        }

        private static int DevidePointsSet(List<Point> points, List<Point> ySeries, 
                                           out List<Point> leftPoints, out List<Point> rightPoints, 
                                           out List<Point> leftY, out List<Point> rightY)
        {
            int lIndex = points.Count/2 + 1;

            leftPoints = new List<Point>();
            rightPoints = new List<Point>();

            //Разбиение на правое и левое подмножества
            for (int i = 0; i < points.Count; ++i)
            {
                if (i < lIndex)
                    leftPoints.Add(points[i]);
                else
                    rightPoints.Add(points[i]);
            }

            // Make sorting list of Y coordinates
            leftY = new List<Point>();
            rightY = new List<Point>();
            for (int i = 0; i < ySeries.Count; ++i)
                if (leftPoints.Contains(ySeries[i]))
                    leftY.Add(ySeries[i]);
                else
                    rightY.Add(ySeries[i]);
            return lIndex;
        }

        private static double FindInDeltaField(List<Point> points, List<Point> ySeries, 
                                               int lIndex, double delta, 
                                               ref Point point1, ref Point point2)
        {
            List<Point> yLine = new List<Point>();
            double d1 = points[lIndex].X - delta, d2 = points[lIndex].X + delta;

            for (int i = 0; i < ySeries.Count; ++i)
            {
                if (ySeries[i].X >= d1 && ySeries[i].X <= d2)
                    yLine.Add(ySeries[i]);
            }

            for (int i = 0; i < yLine.Count; ++i)
            {
                Point p1 = yLine[i];
                for (int j = i + 1; j < yLine.Count && (j - i) <= 7; ++j)
                {
                    Point p2 = yLine[j];
                    double distance = SquareDistance(p1, p2);
                    if (distance < delta)
                    {
                        delta = distance;
                        point1 = p1;
                        point2 = p2;
                    }
                }
            }
            return delta;
        }

        #endregion Find closest points functions

        #region Find farthest points functions
        private static void FindFarthestPoints(IList<Point> pts, out Point point1, out Point point2)
        {
            Point[][] l_u = ConvexHull_LU(pts.ToArray());
            Point[] lower = l_u[0];
            Point[] upper = l_u[1];
            int i = 0;
            int j = lower.Length - 1;

            List<PointPair> pairs = new List<PointPair>(lower.Length + upper.Length);

            while (i < upper.Length - 1 || j > 0)
            {
                pairs.Add(new PointPair(upper[i], lower[j]));

                if (i == upper.Length - 1) j--;
                else if (j == 0) i += 1;
                else if ((upper[i + 1].Y - upper[i].Y) * (lower[j].X - lower[j - 1].X) >
                    (lower[j].Y - lower[j - 1].Y) * (upper[i + 1].X - upper[i].X))
                    i++;
                else
                    j--;
            }

            double distance = SquareDistance(pairs[0].Point1, pairs[0].Point2);
            point1 = pairs[0].Point1;
            point2 = pairs[0].Point2;

            for (int index = 1; index < pairs.Count; ++index)
            {
                var pair = pairs[i];
                double d = SquareDistance(pair.Point1, pair.Point2);
                if (d > distance)
                {
                    distance = d;
                    point1 = pair.Point1;
                    point2 = pair.Point2;
                }
            }
        }

        private static Point[][] ConvexHull_LU(Point[] arr_pts)
        {
            var u = new List<Point>();
            var l = new List<Point>();
            var pts = new List<Point>(arr_pts.Length);
            pts.AddRange(arr_pts);
            pts.Sort(Compare);
            foreach (Point p in pts)
            {
                while (u.Count > 1 && Orientation(At(u, -2), At(u, -1), p) <= 0) Pop(u);
                while (l.Count > 1 && Orientation(At(l, -2), At(l, -1), p) >= 0) Pop(l);
                u.Add(p);
                l.Add(p);
            }
            return new Point[][] { l.ToArray(), u.ToArray() };
        }

        private static T At<T>(List<T> l, int index)
        {
            int n = l.Count;
            if (index < 0)
                return l[n + index];
            return l[index];
        }

        private static double Orientation(Point p, Point q, Point r)
        {
            return (q.Y - p.Y) * (r.X - p.X) - (q.X - p.X) * (r.Y - p.Y);
        }

        private static void Pop<T>(List<T> l)
        {
            int n = l.Count;
            l.RemoveAt(n - 1);
        }

        private static int Compare(Point a, Point b)
        {
            if (a.X < b.X)
            {
                return -1;
            }
            if (a.X == b.X)
            {
                if (a.Y < b.Y)
                    return -1;
                if (a.Y == b.Y)
                    return 0;
            }
            return 1;
        } 
        #endregion

        private static FindPairResult RunPointsCalculation(IList<Point> points,
                                                           Func<IList<Point>, FindPairResult> calculator)
        {
            Contract.Requires<ArgumentNullException>(points != null, "points");
            Contract.Requires<ArgumentOutOfRangeException>(points.Count > 0);

            switch (points.Count)
            {
                case 1: return new FindPairResult(points[0], points[0]);
                case 2: return new FindPairResult(points[0], points[1]);
                default: return calculator(points);
            }
        }

        internal static double SquareDistance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;

        }

        private struct PointPair
        {
            public PointPair(Point point1, Point point2)
                : this()
            {
                Point1 = point1;
                Point2 = point2;
            }

            public Point Point1 { get; private set; }
            public Point Point2 { get; private set; }
        }
    }
}