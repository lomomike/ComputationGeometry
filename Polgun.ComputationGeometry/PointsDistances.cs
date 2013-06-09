using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
            Contract.Requires<ArgumentNullException>(points != null, "points");
            Contract.Requires<ArgumentOutOfRangeException>(points.Count > 0);

            switch (points.Count)
            {
                case 1:
                    return new FindPairResult(points[0], points[0]);
                case 2:
                    return new FindPairResult(points[0], points[1]);
                default:
                    return new FarthestPointsFinder(points).Find();
            }
        }

        #region Find farthest points functions

        #endregion

        /// <summary>
        /// Find square of Euclidean distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point</param>
        /// <returns>Square of distance.</returns>
        internal static double SquareDistance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;

        }
    }


}