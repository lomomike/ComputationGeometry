using System;
using System.Collections.Generic;

namespace Polgun.ComputationGeometry
{
    public static class PointsDistances
    {
        public static FindPairResult FindClosestPair(IList<Point> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (points.Count == 0)
                throw new ArgumentOutOfRangeException("points", "points length is 0");

            if (points.Count == 1)
            {
                return new FindPairResult(points[0], points[0]);
            }
            else if (points.Count == 2)
            {
                return new FindPairResult(points[0], points[1]);
            }
            else // Срабатывает алгоритм расчета
            {

                List<Point> pnts = new List<Point>(points);
                List<Point> ySeries = new List<Point>(points);
                pnts.Sort((first, second) => first.X.CompareTo(second.X));
                ySeries.Sort((first, second) => first.Y.CompareTo(second.Y));

                Point point1;
                Point point2;
                FindClosestPoints(pnts, ySeries, out point1, out point2);
                return new FindPairResult(point1, point2);
            }
        }

        private static double FindClosestPoints(List<Point> points, List<Point> ySeries, out Point point1, out Point point2)
        {
            //Если количество точек <= 3,
            // то находим кратчайшее расстояние простым перебором
            // Для оптимизации расчетов вычисляем не расстояния, а квадраты расстояний
            if (points.Count <= 3)
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
            else // В наборе точек больше 3х, будем проводить деление и рекурсивный вызов
            {
                //Находим вертикальную прямую l, которая делит множество точек points на два подмножества.
                int lIndex = points.Count / 2 + 1;

                List<Point> leftPoints = new List<Point>();
                List<Point> rightPoints = new List<Point>();

                //Разбиение на правое и левое подмножества
                for (int i = 0; i < points.Count; ++i)
                {
                    if (i < lIndex)
                        leftPoints.Add(points[i]);
                    else
                        rightPoints.Add(points[i]);
                }

                //Формирование отсортированного списка координат Y
                List<Point> leftY = new List<Point>();
                List<Point> rightY = new List<Point>();
                for (int i = 0; i < ySeries.Count; ++i)
                    if (leftPoints.Contains(ySeries[i]))
                        leftY.Add(ySeries[i]);
                    else
                        rightY.Add(ySeries[i]);

                Point ind11, ind12, ind21, ind22;
                double delta1 = FindClosestPoints(leftPoints, leftY, out ind11, out ind12);
                double delta2 = FindClosestPoints(rightPoints, rightY, out ind21, out ind22);

                //Находи наименьшее расстояние из пары
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

                // Проверка наличия ближайшего расстояния в полосе 2*delta;

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
        }

        internal static double SquareDistance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;

        } 
    }
}