using NUnit.Framework;

namespace Polgun.ComputationGeometry.Tests
{
    [TestFixture]
    public class SizeTests
    {
        const double width = 100.0;
        const double height = 50.0;

        private Size size;

        [SetUp]
        public void Setup()
        {
            size = new Size(width, height);
        }

        [Test]
        public void TestSizeCreation()
        {
            Assert.That(size.Width, Is.EqualTo(width));
            Assert.That(size.Height, Is.EqualTo(height));
        }

        [Test]
        public void TestSizeAddition()
        {
            Size secondSize = new Size(10, 50);
            Size newSize = size + secondSize;

            Assert.That(newSize.Width, Is.EqualTo(110.0));
            Assert.That(newSize.Height, Is.EqualTo(100.0));
        }

        [Test]
        public void TestSizeSubstraction()
        {
            Size secondSize = new Size(10, 50);
            Size newSize = size - secondSize;

            Assert.That(newSize.Width, Is.EqualTo(90.0));
            Assert.That(newSize.Height, Is.EqualTo(0.0));
        }

        [Test]
        public void TestSizesEqual()
        {
            Size notEqualSize = new Size(10, 50);
            Size equalSize = new Size(size);

            Assert.That(size == equalSize, Is.True);
            Assert.That(size.Equals(equalSize), Is.True);
            Assert.That(size != notEqualSize, Is.True);
        }
    }
}