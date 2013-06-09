using System.Collections.Generic;

namespace Polgun.ComputationGeometry
{
    /// <summary>
    /// Uncludes implementation of searching convex hull algorithms.
    /// </summary>
    public static class ConvexHull
    {
        /// <summary>
        /// Implementation of Graham comvex hull serach algorithm.
        /// </summary>
        /// <param name="points">All points on plane.</param>
        /// <returns>The set of points, that are the convex hull.</returns>
         public static IEnumerable<Point> Graham(IEnumerable<Point> points)
         {
             return new GrahamHullFinder(points).Find();
         }
    }
}