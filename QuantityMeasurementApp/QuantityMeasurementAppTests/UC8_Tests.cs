using NUnit.Framework;
using QM.Models.Models;
using QM.BusinessLogic.Service;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    public class UC8Tests
    {
        private const double EPSILON = 0.001;

        [Test]
        public void testLengthUnitEnum_FeetConstant()
        {
            Assert.That(LengthUnit.FEET.GetConversionFactor(), Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_InchesConstant()
        {
            Assert.That(LengthUnit.INCHES.GetConversionFactor(), Is.EqualTo(1.0 / 12.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_YardsConstant()
        {
            Assert.That(LengthUnit.YARDS.GetConversionFactor(), Is.EqualTo(3.0).Within(EPSILON));
        }

        [Test]
        public void testLengthUnitEnum_CentimetersConstant()
        {
            Assert.That(LengthUnit.CENTIMETERS.GetConversionFactor(), Is.EqualTo(1.0 / 30.48).Within(EPSILON));
        }

        [Test]
        public void testConvertToBaseUnit_InchesToFeet()
        {
            double result = LengthUnit.INCHES.ConvertToBaseUnit(12.0);
            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testConvertToBaseUnit_YardsToFeet()
        {
            double result = LengthUnit.YARDS.ConvertToBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(3.0).Within(EPSILON));
        }

        [Test]
        public void testConvertFromBaseUnit_FeetToInches()
        {
            double result = LengthUnit.INCHES.ConvertFromBaseUnit(1.0);
            Assert.That(result, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testConvertFromBaseUnit_FeetToYards()
        {
            double result = LengthUnit.YARDS.ConvertFromBaseUnit(3.0);
            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testQuantityLengthRefactored_Equality()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            Assert.That(LengthService.AreEqual(a, b), Is.True);
        }

        [Test]
        public void testQuantityLengthRefactored_ConvertTo()
        {
            var q = new QuantityLength(1.0, LengthUnit.FEET);
            var result = LengthService.Convert(q, LengthUnit.INCHES);

            Assert.That(result.Value, Is.EqualTo(12.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void testQuantityLengthRefactored_Add()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = LengthService.Add(a, b, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testQuantityLengthRefactored_AddWithTargetUnit()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = LengthService.Add(a, b, LengthUnit.YARDS);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(0.01));
        }

        [Test]
        public void testRoundTripConversion_RefactoredDesign()
        {
            var q = new QuantityLength(5.0, LengthUnit.FEET);

            var inches = LengthService.Convert(q, LengthUnit.INCHES);
            var back = LengthService.Convert(inches, LengthUnit.FEET);

            Assert.That(back.Value, Is.EqualTo(q.Value).Within(EPSILON));
        }
    }
}