using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Polgun.ComputationGeometry.Tests
{
    [TestFixture]
    public class PointsDistancesTests
    {
        #region FindClosestPoint Tests
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFindClosestInNullList()
        {
            PointsDistances.FindClosestPair(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFindClosestInEmptyList()
        {
            PointsDistances.FindClosestPair(new Point[0]);
        }

        [Test]
        public void TestFindClosestInOnePoint()
        {
            Point point = new Point(100, 100);

            FindPairResult result = PointsDistances.FindClosestPair(new[] { point });

            Assert.That(result.Point1, Is.EqualTo(point));
            Assert.That(result.Point2, Is.EqualTo(point));
            Assert.That(result.Distance, Is.EqualTo(0.0));
        }

        [Test]
        public void TestFindClosestInTwoPints()
        {
            Point point1 = new Point(10, 0);
            Point point2 = new Point(20, 0);

            FindPairResult result = PointsDistances.FindClosestPair(new[] { point1, point2 });

            Assert.That(result.Point1, Is.EqualTo(point1));
            Assert.That(result.Point2, Is.EqualTo(point2));
            Assert.That(result.Distance, Is.EqualTo(10.0));
        }

        [Test]
        public void TestFindClosestInThreePoints()
        {
            Point point1 = new Point(10, 0);
            Point point2 = new Point(20, 0);
            Point point3 = new Point(100, 100);

            FindPairResult result = PointsDistances.FindClosestPair(new[] { point1, point2, point3 });

            Assert.That(result.Point1, Is.EqualTo(point1));
            Assert.That(result.Point2, Is.EqualTo(point2));
            Assert.That(result.Distance, Is.EqualTo(10.0));
        }

        [Test]
        public void TestFindClosestInTenRandomPoints()
        {
            List<Point> points = Enumerable.Range(0, 10)
                                           .Select(a => new Point(a * a, a * a))
                                           .ToList();

            FindPairResult expectedResult = BruteForceClosestPoints(points);

            FindPairResult result = PointsDistances.FindClosestPair(points);



            AssertResult(expectedResult, result);

        } 
        #endregion

        #region FindFarthestPoint Tests
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFindFarthestInNullList()
        {
            PointsDistances.FindFarthestPair(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFindFarthestInEmptyList()
        {
            PointsDistances.FindFarthestPair(new Point[0]);
        }

        [Test]
        public void TestFindFarthestInOnePoint()
        {
            Point point = new Point(100, 100);

            FindPairResult result = PointsDistances.FindFarthestPair(new[] { point });

            Assert.That(result.Point1, Is.EqualTo(point));
            Assert.That(result.Point2, Is.EqualTo(point));
            Assert.That(result.Distance, Is.EqualTo(0.0));
        }

        [Test]
        public void TestFindFarthestInTwoPints()
        {
            Point point1 = new Point(10, 0);
            Point point2 = new Point(20, 0);

            FindPairResult result = PointsDistances.FindFarthestPair(new[] { point1, point2 });

            Assert.That(result.Point1, Is.EqualTo(point1));
            Assert.That(result.Point2, Is.EqualTo(point2));
            Assert.That(result.Distance, Is.EqualTo(10.0));
        }

        [Test]
        public void TestFindFarthestInThreePoints()
        {
            Point point1 = new Point(10, 0);
            Point point2 = new Point(20, 0);
            Point point3 = new Point(100, 100);

            FindPairResult result = PointsDistances.FindFarthestPair(new[] { point1, point2, point3 });

            Assert.That(result.Point1, Is.EqualTo(point1));
            Assert.That(result.Point2, Is.EqualTo(point3));
        }

        [Test]
        public void TestFindFarthestInTenPoints()
        {
            List<Point> points = Enumerable.Range(0, 10)
                                           .Select(a => new Point(-1 * a * a, -1 * a * a))
                                           .ToList();
            FindPairResult expectedResult = new FindPairResult(points.First(), points.Last());

            FindPairResult result = PointsDistances.FindFarthestPair(points);

            AssertResult(expectedResult, result);

        } 
        

        #endregion

        private void AssertResult(FindPairResult expected, FindPairResult actual)
        {
            //Assert.That(actual.Point1, Is.EqualTo(expected.Point1));
            //Assert.That(actual.Point2, Is.EqualTo(expected.Point2));
            AssertMinimalPoints(expected, actual);
            AssertMaximalPoints(expected, actual);
            Assert.That(actual.Distance, Is.EqualTo(expected.Distance));
        }

        private void AssertMaximalPoints(FindPairResult expected, FindPairResult actual)
        {
            Point expectedMax = Max(expected.Point1, expected.Point2);
            Point actualMax = Max(actual.Point1, actual.Point2);

            Assert.That(actualMax, Is.EqualTo(expectedMax));
        }

        private void AssertMinimalPoints(FindPairResult expected, FindPairResult actual)
        {
            Point expectedMin = Min(expected.Point1, expected.Point2);
            Point actualMin = Min(actual.Point1, actual.Point2);

            Assert.That(actualMin, Is.EqualTo(expectedMin));
        }

        private Point Min(Point lvalue, Point rvalue)
        {
              if (lvalue.X < rvalue.X)
            {
                return lvalue;
            }
            else if (lvalue.X == rvalue.X)
            {
                if (lvalue.Y < rvalue.Y || lvalue.Y == rvalue.Y)
                    return lvalue;
            }
            return rvalue;
        }

        private Point Max(Point lvalue, Point rvalue)
        {
            if (lvalue.X < rvalue.X)
            {
                return rvalue;
            }
            else if (lvalue.X == rvalue.X)
            {
                if (lvalue.Y < rvalue.Y || lvalue.Y == rvalue.Y)
                    return rvalue;
            }
            return lvalue;
        }

        private FindPairResult BruteForceClosestPoints(IList<Point> points)
        {
            double minDistance = double.MaxValue;
            Point point1 = default(Point);
            Point point2 = default(Point);

            for (int i = 0; i < points.Count - 1; ++i)
                for (int j = i + 1; j < points.Count; ++j)
                {
                    double currentDistance = SquareDistance(points[i], points[j]);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        point1 = points[i];
                        point2 = points[j];
                    }
                }
            return new FindPairResult(point1, point2);
        }

        private double SquareDistance(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;

        } 
    }
}