using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementAppTests
{
    public class VolumeUnitTests
    {
        [Test]
        public void GivenSameLitreValues_WhenCompared_ShouldReturnTrue()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void GivenLitreAndMillilitre_WhenEquivalent_ShouldReturnTrue()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void GivenDifferentLitreValues_WhenCompared_ShouldReturnFalse()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Assert.That(v1.Equals(v2), Is.False);
        }

        [Test]
        public void GivenLitre_WhenConvertedToMillilitre_ShouldReturn1000()
        {
            var v = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            var result = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.That(result.Value, Is.EqualTo(1000.0).Within(0.001));
        }

        [Test]
        public void GivenGallon_WhenConvertedToLitre_ShouldReturn3Point78541()
        {
            var v = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);

            var result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(3.78541).Within(0.001));
        }

        [Test]
        public void GivenTwoLitreValues_WhenAdded_ShouldReturnThreeLitre()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);

            var result = v1.Add(v2, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(3.0).Within(0.001));
        }

        [Test]
        public void GivenLitreAndMillilitre_WhenAdded_ShouldReturnTwoLitre()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(0.001));
        }

        [Test]
        public void GivenExplicitTargetUnit_WhenAdded_ShouldReturnMillilitre()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2, VolumeUnit.MILLILITRE);

            Assert.That(result.Value, Is.EqualTo(2000.0).Within(0.001));
        }

        [Test]
        public void GivenZeroValue_WhenConverted_ShouldRemainZero()
        {
            var v = new Quantity<VolumeUnit>(0.0, VolumeUnit.LITRE);

            var result = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.That(result.Value, Is.EqualTo(0));
        }

        [Test]
        public void GivenNegativeVolume_WhenCompared_ShouldReturnTrue()
        {
            var v1 = new Quantity<VolumeUnit>(-1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(-1000.0, VolumeUnit.MILLILITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }
    }
}