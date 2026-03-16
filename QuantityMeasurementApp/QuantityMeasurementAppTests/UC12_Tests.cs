using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class QuantityTests
    {
        [Test]
        public void AddLengthQuantities_ShouldReturnCorrectSum()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(24, LengthUnit.INCHES);

            var result = q1.Add(q2, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(4.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void SubtractWeightQuantities_ShouldReturnCorrectDifference()
        {
            var q1 = new Quantity<WeightUnit>(5, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(2000, WeightUnit.GRAM);

            var result = q1.Subtract(q2, WeightUnit.KILOGRAM);

            Assert.That(result.Value, Is.EqualTo(3.0));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        [Test]
        public void DivideVolumeQuantities_ShouldReturnCorrectQuotient()
        {
            var q1 = new Quantity<VolumeUnit>(4, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(2, VolumeUnit.LITRE);

            double quotient = q1.Divide(q2);

            Assert.That(quotient, Is.EqualTo(2.0));
        }

        [Test]
        public void CompareQuantities_ShouldReturnTrueForEqualValues()
        {
            var q1 = new Quantity<LengthUnit>(36, LengthUnit.INCHES);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.FEET);

            bool equal = q1.Equals(q2);

            Assert.That(equal, Is.True);
        }

        [Test]
        public void ConvertQuantity_ShouldReturnCorrectValue()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.YARDS);

            var converted = q.ConvertTo(LengthUnit.INCHES);

            Assert.That(converted.Value, Is.EqualTo(36));
            Assert.That(converted.Unit, Is.EqualTo(LengthUnit.INCHES));
        }
    }
}