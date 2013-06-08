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
            Contract.Requires<ArgumentNullException>(points != null, "points");
            Contract.Requires<ArgumentOutOfRangeException>(points.Count > 0);

            switch (points.Count)
            {
                case 1:
                    return new FindPairResult(points[0], points[0]);
                case 2:
                    return new FindPairResult(points[0], points[1]);
                default:

                    return new ClosestPointsFinder(points).Find();
            }
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