using NUnit.Framework;

namespace Polgun.ComputationGeometry.Tests
{
    [TestFixture]
    public class PointTests
    {
        private const double x = 100.0;
        private const double y = 50.0;

        private Point point;

        [SetUp]
        public void SetUp()
        {
            point = new Point(x, y);
        }

        [Test]
        public void TestPointCreation()
        {
            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
            Assert.That(point.IsEmpty, Is.False);
        }

        [Test]
        public void TestPointEquals()
        {
            Point equalPoint = new Point(x, y);
            Point notEqualPoint = new Point(0, 0);

            Assert.That(point == equalPoint, Is.True);
            Assert.That(point.Equals(equalPoint), Is.True);
            Assert.That(point != notEqualPoint, Is.True);
        }

        [Test]
        public void TestPoinAddition()
        {
            Point newPoint = point + new Size(5.0, 10.0);

            Assert.That(newPoint.X, Is.EqualTo(105.0));
            Assert.That(newPoint.Y, Is.EqualTo(60.0));
        }

        [Test]
        public void TestPoinSubstraction()
        {
            Point newPoint = point - new Size(5.0, 10.0);

            Assert.That(newPoint.X, Is.EqualTo(95.0));
            Assert.That(newPoint.Y, Is.EqualTo(40.0));
        }
    }
}