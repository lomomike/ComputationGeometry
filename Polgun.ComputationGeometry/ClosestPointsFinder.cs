using System;
using System.Collections.Generic;

namespace Polgun.ComputationGeometry
{
    internal class ClosestPointsFinder
    {
        private readonly List<Point> _xSeries; // Ordered by X series of xSeries
        private readonly List<Point> _ySeries;

        private double _distance; // Distance betwee two result xSeries
        private Point _point1; // First result point
        private Point _point2; // Second result point

        // Points seieses in devision
        private List<Point> _leftXSeries;
        private List<Point> _rightXSeries;

        private List<Point> _leftYSeries;
        private List<Point> _rightYSeries;


        public ClosestPointsFinder(IEnumerable<Point> points)
        {
            _xSeries = new List<Point>(points);
            _xSeries.Sort((first, second) => first.X.CompareTo(second.X));

            _ySeries = new List<Point>(points);
            _ySeries.Sort((first, second) => first.Y.CompareTo(second.Y));
        }

        public FindPairResult Find()
        {
            DevideAndFind(_xSeries, _ySeries);
            return new FindPairResult(_point1, _point2);
        }


        private Tuple<Point, Point, double> DevideAndFind(List<Point> xSeries, List<Point> ySeries)
        {
            // If count of xSeries is less or equals 3,
            // find the shortest distance with simple search.
            // Calculate the squares of distances to optimize the calculation
            if (xSeries.Count <= 3)
            {
                SearchClosestInLittleSequrnce(xSeries);
                return new Tuple<Point, Point, double>(_point1, _point2, _distance);
            }

            // Find vertical line l, wich devides the set of xSeries to two subsets
            var lIndex = DevidePointsSet(xSeries, ySeries);

            // Make recursive call
            var result1 = DevideAndFind(_leftXSeries, _leftYSeries);
            var result2 = DevideAndFind(_rightXSeries, _rightYSeries);
            ChooseClosestResult(result1, result2);


            // Find the shortest distance in the 2 * delta field
            return FindInDeltaField(xSeries, ySeries, lIndex);
        }

        private void SearchClosestInLittleSequrnce(List<Point> xSeries)
        {
            double minDistance = double.MaxValue;
            _point1 = _point2 = default(Point);

            for (int i = 0; i < xSeries.Count - 1; ++i)
                for (int j = i + 1; j < xSeries.Count; ++j)
                {
                    double distance = PointsDistances.SquareDistance(xSeries[i], xSeries[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _point1 = xSeries[i];
                        _point2 = xSeries[j];
                    }
                }

            _distance = minDistance;
        }

        private int DevidePointsSet(List<Point> xSeries, List<Point> ySeries)
        {
            int lIndex = xSeries.Count / 2 + 1;

            _leftXSeries = new List<Point>();
            _rightXSeries = new List<Point>();

            // Devide xSeries set to left and right subsets
            for (int i = 0; i < xSeries.Count; ++i)
            {
                if (i < lIndex)
                    _leftXSeries.Add(xSeries[i]);
                else
                    _rightXSeries.Add(xSeries[i]);
            }

            // Make sorting list of Y coordinates
            _leftYSeries = new List<Point>();
            _rightYSeries = new List<Point>();
            for (int i = 0; i < ySeries.Count; ++i)
                if (_leftXSeries.Contains(ySeries[i]))
                    _leftYSeries.Add(ySeries[i]);
                else
                    _rightYSeries.Add(ySeries[i]);
            return lIndex;
        }

        private Tuple<Point, Point, double> FindInDeltaField(List<Point> xSeries, List<Point> ySeries, int lIndex)
        {
            List<Point> yLine = new List<Point>();
            double d1 = xSeries[lIndex].X - _distance;
            double d2 = xSeries[lIndex].X + _distance;

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
                    double squareDistance = PointsDistances.SquareDistance(p1, p2);
                    if (squareDistance < _distance)
                    {
                        _distance = squareDistance;
                        _point1 = p1;
                        _point2 = p2;
                    }
                }
            }
            return new Tuple<Point, Point, double>(_point1, _point2, _distance);
        }

        private void ChooseClosestResult(Tuple<Point, Point, double> tuple1, Tuple<Point, Point, double> tuple2)
        {
            // delta1 > delta2
            if (tuple1.Item3 > tuple2.Item3)
            {
                _distance = tuple1.Item3;
                _point1 = tuple1.Item1;
                _point2 = tuple1.Item2;
            }
            else
            {
                _distance = tuple2.Item3;
                _point1 = tuple2.Item1;
                _point2 = tuple2.Item2;
            }
        }
    }
}