using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class FeetTests
    {
        [Test]
        public void GivenSameFeetValues_ShouldReturnTrue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            Assert.That(f1.Equals(f2), Is.True);
        }

        [Test]
        public void GivenDifferentFeetValues_ShouldReturnFalse()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);

            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        public void GivenFeetComparedWithNull_ShouldReturnFalse()
        {
            Feet f1 = new Feet(1.0);

            Assert.That(f1.Equals(null), Is.False);
        }

        [Test]
        public void GivenSameReference_ShouldReturnTrue()
        {
            Feet f1 = new Feet(1.0);

            Assert.That(f1.Equals(f1), Is.True);
        }

        [Test]
        public void GivenDifferentTypeObject_ShouldReturnFalse()
        {
            Feet f1 = new Feet(1.0);
            object obj = "NotFeet";

            Assert.That(f1.Equals(obj), Is.False);
        }
    }
}