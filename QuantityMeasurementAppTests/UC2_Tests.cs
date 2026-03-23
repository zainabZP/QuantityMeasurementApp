using NUnit.Framework;
using QuantityMeasurementApp;
using QM.Models.Models;

namespace QuantityMeasurementApp.Tests
{
    public class MeasurementTests
    {
        [Test]
        public void GivenSameFeetValues_ShouldReturnTrue()
        {
            Assert.That(Program.CheckFeetEquality(1.0, 1.0), Is.True);
        }

        [Test]
        public void GivenDifferentFeetValues_ShouldReturnFalse()
        {
            Assert.That(Program.CheckFeetEquality(1.0, 2.0), Is.False);
        }

        [Test]
        public void GivenSameInchValues_ShouldReturnTrue()
        {
            Assert.That(Program.CheckInchEquality(5.0, 5.0), Is.True);
        }

        [Test]
        public void GivenDifferentInchValues_ShouldReturnFalse()
        {
            Assert.That(Program.CheckInchEquality(5.0, 7.0), Is.False);
        }

        [Test]
        public void GivenSameReference_ShouldReturnTrue()
        {
            Feet f = new Feet(2.0);
            Assert.That(f.Equals(f), Is.True);
        }
    }
}