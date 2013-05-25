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
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUnitOfWork()
        {
            PointsDistances.FindClosestPair(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFindClosestinEmptyList()
        {
            PointsDistances.FindClosestPair(new Point[0]);
        }

        [Test]
        public void TestFindClosestInOnePoint()
        {
            Point point = new Point(100,100);

            FindPairResult result = PointsDistances.FindClosestPair(new[] {point});

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

        private void AssertResult(FindPairResult expected, FindPairResult actual)
        {
            Assert.That(actual.Point1, Is.EqualTo(expected.Point1));
            Assert.That(actual.Point2, Is.EqualTo(expected.Point2));
            Assert.That(actual.Distance, Is.EqualTo(expected.Distance));
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