using System.Collections.Generic;
using System.Linq;

namespace Polgun.ComputationGeometry
{
    internal class FarthestPointsFinder
    {
        private readonly Point[] _points;

        public FarthestPointsFinder(IEnumerable<Point> points)
        {
            _points = points.ToArray();
        }

        public FindPairResult Find()
        {
            Point[][] l_u = ConvexHull_LU(_points);
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

            double distance = PointsDistances.SquareDistance(pairs[0].Point1, pairs[0].Point2);
            Point point1 = pairs[0].Point1;
            Point point2 = pairs[0].Point2;

            for (int index = 1; index < pairs.Count; ++index)
            {
                var pair = pairs[i];
                double d = PointsDistances.SquareDistance(pair.Point1, pair.Point2);
                if (d > distance)
                {
                    distance = d;
                    point1 = pair.Point1;
                    point2 = pair.Point2;
                }
            }

            return new FindPairResult(point1, point2);
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

        private Point[][] ConvexHull_LU(Point[] origitaPoints)
        {
            var u = new List<Point>();
            var l = new List<Point>();
            var points = new List<Point>(origitaPoints.Length);
            points.AddRange(origitaPoints);
            points.Sort(Compare);
            foreach (Point p in points)
            {
                while (u.Count > 1 && Orientation(At(u, -2), At(u, -1), p) <= 0) Pop(u);
                while (l.Count > 1 && Orientation(At(l, -2), At(l, -1), p) >= 0) Pop(l);
                u.Add(p);
                l.Add(p);
            }
            return new Point[][] { l.ToArray(), u.ToArray() };
        }

        private Point At(List<Point> l, int index)
        {
            int n = l.Count;
            if (index < 0)
                return l[n + index];
            return l[index];
        }

        private double Orientation(Point p, Point q, Point r)
        {
            return (q.Y - p.Y) * (r.X - p.X) - (q.X - p.X) * (r.Y - p.Y);
        }

        private static void Pop(List<Point> l)
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

    }
}