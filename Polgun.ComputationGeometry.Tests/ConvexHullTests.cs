﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Polgun.ComputationGeometry.Tests
{
    [TestFixture]
    public class ConvexHullTests
    {
        [Test]
        [ExpectedException]
        public void TestGrahamSearchNullList()
        {
            ConvexHull.Graham(null);
        }

        [Test]
        [ExpectedException]
        public void TestGrahamSearchInOnePoint()
        {
            ConvexHull.Graham(new[] { new Point(0, 0) });
        }

        [Test]
        public void TestGrahamSearchInTwoPoints()
        {
            List<Point> points = new List<Point> { new Point(0, 0), new Point(1, 1) };
            var hull = ConvexHull.Graham(points);

            CollectionAssert.AreEquivalent(hull, points);
        }

        [Test]
        public void TestGrahamSearchInTenPoints()
        {
            var expectedHull = new List<Point>
                {
                    new Point(10, 10),
                    new Point(5, 20),
                    new Point(10, 30),

                    new Point(20, 33),
                    new Point(20, 5),

                    new Point(30, 30),
                    new Point(33, 20),
                    new Point(30, 10),

                };

            var hull = ConvexHull.Graham(expectedHull.Concat(new [] {new Point(15,15), 
                                                                     new Point(25,25) }));

            CollectionAssert.AreEquivalent(expectedHull, hull);
        }

        
    }
}