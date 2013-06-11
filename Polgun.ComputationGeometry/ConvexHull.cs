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

        public static IEnumerable<Point> Jarvis(IEnumerable<Point> points)
        {
            List<Point> pointsBufer = new List<Point>(points);
            List<Point> result = new List<Point>();

            int it = 0;
            int leftDownIt = 0;
            Point leftDownPoint = pointsBufer[0];
            Point currentPoint;
            for (it = 1; it < pointsBufer.Count; ++it)
            {
               currentPoint = pointsBufer[it];
                if (currentPoint.X < leftDownPoint.X)
                {
                    leftDownPoint = currentPoint;
                    leftDownIt = it;
                }
                else if (currentPoint.X == leftDownPoint.X && 
                         currentPoint.Y > leftDownPoint.Y)
                {
                    leftDownPoint = currentPoint;
                    leftDownIt = it;
                }
            }
            
            pointsBufer.RemoveAt(leftDownIt);
            result.Add(leftDownPoint);

            currentPoint = leftDownPoint;

            Point nextPoint = Point.Empty, tryPoint;
            int nextIt = -1;

            do {

                        //если currentPoint == leftDownPoint то следующий фор сменит nextPoint
                        //если фор не сменит leftDownPoint - полигон замкнулся - break
                        if (determinantSignum(nextPoint, currentPoint, leftDownPoint)) {
                                nextPoint = leftDownPoint;
                                nextIt = it;
                        }

                        for (it = 0; it != pointsBufer.Count; it++) {
                            tryPoint = pointsBufer[it];
                                //если поворот вправо
                                if (determinantSignum(nextPoint, currentPoint, tryPoint)) {
                                        nextPoint = tryPoint;
                                        nextIt = it;
                                }
                        }

                if (nextIt >= 0 && nextIt < pointsBufer.Count)
                {
                    pointsBufer.RemoveAt(nextIt);
                    result.Add(nextPoint);
                    currentPoint = nextPoint;
                }

            } while(nextPoint != leftDownPoint);

            return result;

            //vector<Point> srcCopy = src;

            //   //найдем самую нижнюю точку
            //   //если их несколько - выберем самую левую
            //   //такая политика обеспечит более удачную сложность в общем случае O(hn)
            //   vector<Point>::iterator it;
            //   vector<Point>::iterator leftDownIt = srcCopy.begin();
            //   Point currentPoint;
            //   Point leftDownPoint = *(leftDownIt);
            //   for (it = srcCopy.begin() + 1; it != srcCopy.end(); it++) {
            //           currentPoint = *it;
            //           if (currentPoint.first < leftDownPoint.first) {
            //                   leftDownPoint = currentPoint;
            //                   leftDownIt = it;
            //           } else if (currentPoint.first == leftDownPoint.first) {
            //                   if (currentPoint.second > leftDownPoint.second) {
            //                           leftDownPoint = currentPoint;
            //                           leftDownIt = it;
            //                   }
            //           }
            //   }
            //   //удалим эту точку и будем использовать ее как стартовую
            //   srcCopy.erase(leftDownIt);
            //   this->pts->push_back(leftDownPoint);

            //   //берем точку currentPoint и просматриваем относительно нее все точки
            //   currentPoint = leftDownPoint;

            //   Point nextPoint, tryPoint;
            //   vector<Point>::iterator nextIt;
            //   do {

            //           //если currentPoint == leftDownPoint то следующий фор сменит nextPoint
            //           //если фор не сменит leftDownPoint - полигон замкнулся - break
            //           if (determinantSignum(nextPoint, currentPoint, leftDownPoint)) {
            //                   nextPoint = leftDownPoint;
            //                   nextIt = it;
            //           }

            //           for (it = srcCopy.begin(); it != srcCopy.end(); it++) {
            //                   tryPoint = *it;
            //                   //если поворот вправо
            //                   if (determinantSignum(nextPoint, currentPoint, tryPoint)) {
            //                           nextPoint = tryPoint;
            //                           nextIt = it;
            //                   }
            //           }

            //           srcCopy.erase(nextIt);
            //           this->pts->push_back(nextPoint);
            //           currentPoint = nextPoint;

            //   } while(nextPoint != leftDownPoint);
        }

        private static bool determinantSignum(Point a, Point b, Point c) {
                return (((a.X - b.X) * (c.Y - b.Y) - (c.X - b.X) * (a.Y - b.Y)) >= 0);
        }
    }
}