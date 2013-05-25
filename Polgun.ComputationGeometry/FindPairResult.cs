using System;

namespace Polgun.ComputationGeometry
{
    /// <summary>
    /// Result of point search algorithms.
    /// </summary>
    public struct FindPairResult
    {
        public FindPairResult(Point point1, Point point2) : this()
        {
            this.Point1 = point1;
            this.Point2 = point2;
            this.Distance = Math.Sqrt(PointsDistances.SquareDistance(point1, point2));
        }

        /// <summary>
        /// First point in result.
        /// </summary>
        public Point Point1 { get; private set; }

        /// <summary>
        /// Second point in result.
        /// </summary>
        public Point Point2 { get; private set; }

        /// <summary>
        /// Distance betwee two result points.
        /// </summary>
        public double Distance { get; private set; }
    }
}