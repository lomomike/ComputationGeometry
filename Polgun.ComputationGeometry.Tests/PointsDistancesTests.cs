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
        public void TestFindClosestInTwoPoints()
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
        public void TestFindClosestInTenPoints()
        {
            List<Point> points = Enumerable.Range(0, 10)
                                           .Select(a => new Point(a * a, a * a))
                                           .ToList();
            var expectedResult = new [] { points[0], points[1] };

            FindPairResult result = PointsDistances.FindClosestPair(points);

            CollectionAssert.AreEquivalent(expectedResult, new[] { result.Point1, result.Point2 });
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
            var expectedResult = new[] { points.First(), points.Last() };

            FindPairResult result = PointsDistances.FindFarthestPair(points);

            CollectionAssert.AreEquivalent(expectedResult, new[] { result.Point1, result.Point2 });
        }


        #endregion
    }
}