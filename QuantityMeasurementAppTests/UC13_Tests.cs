using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class UC13QuantityTests
    {
        // ============================
        // Length Tests
        // ============================

        [Test]
        public void Add_Lengths_ReturnsCorrectResult()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES); // 1 foot

            var result = q1.Add(q2, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(3).Within(0.0001));
        }

        [Test]
        public void Subtract_Lengths_ReturnsCorrectResult()
        {
            var q1 = new Quantity<LengthUnit>(3, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            var result = q1.Subtract(q2, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(2).Within(0.0001));
        }

        [Test]
        public void Divide_Lengths_ReturnsCorrectResult()
        {
            var q1 = new Quantity<LengthUnit>(6, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(3).Within(0.0001));
        }

        [Test]
        public void Compare_Lengths_ReturnsTrue()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.YARDS);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.FEET);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void Convert_Length_ReturnsCorrectResult()
        {
            var q = new Quantity<LengthUnit>(3, LengthUnit.FEET);

            var converted = q.ConvertTo(LengthUnit.INCHES);

            Assert.That(converted.Value, Is.EqualTo(36).Within(0.0001));
        }

        // ============================
        // Weight Tests
        // ============================

        [Test]
        public void Add_Weights_ReturnsCorrectResult()
        {
            var w1 = new Quantity<WeightUnit>(2, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(500, WeightUnit.GRAM);

            var result = w1.Add(w2, WeightUnit.KILOGRAM);

            Assert.That(result.Value, Is.EqualTo(2.5).Within(0.0001));
        }

        [Test]
        public void Subtract_Weights_ReturnsCorrectResult()
        {
            var w1 = new Quantity<WeightUnit>(2, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(500, WeightUnit.GRAM);

            var result = w1.Subtract(w2, WeightUnit.KILOGRAM);

            Assert.That(result.Value, Is.EqualTo(1.5).Within(0.0001));
        }

        [Test]
        public void Divide_Weights_ReturnsCorrectResult()
        {
            var w1 = new Quantity<WeightUnit>(2, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(500, WeightUnit.GRAM);

            double result = w1.Divide(w2);

            Assert.That(result, Is.EqualTo(4).Within(0.0001));
        }

        [Test]
        public void Compare_Weights_ReturnsTrue()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void Convert_Weight_ReturnsCorrectResult()
        {
            var w = new Quantity<WeightUnit>(2, WeightUnit.KILOGRAM);

            var converted = w.ConvertTo(WeightUnit.GRAM);

            Assert.That(converted.Value, Is.EqualTo(2000).Within(0.0001));
        }

        // ============================
        // Volume Tests
        // ============================

        [Test]
        public void Add_Volumes_ReturnsCorrectResult()
        {
            var v1 = new Quantity<VolumeUnit>(2, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(500, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(2.5).Within(0.0001));
        }

        [Test]
        public void Subtract_Volumes_ReturnsCorrectResult()
        {
            var v1 = new Quantity<VolumeUnit>(2, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(500, VolumeUnit.MILLILITRE);

            var result = v1.Subtract(v2, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(1.5).Within(0.0001));
        }

        [Test]
        public void Divide_Volumes_ReturnsCorrectResult()
        {
            var v1 = new Quantity<VolumeUnit>(2, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(500, VolumeUnit.MILLILITRE);

            double result = v1.Divide(v2);

            Assert.That(result, Is.EqualTo(4).Within(0.0001));
        }

        [Test]
        public void Compare_Volumes_ReturnsTrue()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void Convert_Volume_ReturnsCorrectResult()
        {
            var v = new Quantity<VolumeUnit>(2, VolumeUnit.LITRE);

            var converted = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.That(converted.Value, Is.EqualTo(2000).Within(0.0001));
        }
    }
}