using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class UC5_Tests
    {
        [Test]
        public void TestFeetToInches()
        {
            var length = new QuantityLength(1, LengthUnit.FEET);
            double converted = ConversionService.Convert(length.Value, length.Unit, LengthUnit.INCHES);
            Assert.That(converted, Is.EqualTo(12.0));
        }

        [Test]
        public void TestYardsToFeet()
        {
            var length = new QuantityLength(2, LengthUnit.YARDS);
            double converted = ConversionService.Convert(length.Value, length.Unit, LengthUnit.FEET);
            Assert.That(converted, Is.EqualTo(6.0));
        }

        [Test]
        public void TestCentimetersToFeet()
        {
            var length = new QuantityLength(30.48, LengthUnit.CENTIMETERS);
            double converted = ConversionService.Convert(length.Value, length.Unit, LengthUnit.FEET);
            Assert.That(converted, Is.EqualTo(1.0).Within(1e-6));
        }

        [Test]
        public void TestInchesToFeet()
        {
            var length = new QuantityLength(24, LengthUnit.INCHES);
            double converted = ConversionService.Convert(length.Value, length.Unit, LengthUnit.FEET);
            Assert.That(converted, Is.EqualTo(2.0));
        }

        [Test]
        public void TestRoundTripConversion()
        {
            var original = new QuantityLength(3, LengthUnit.FEET);
            double convertedToInches = ConversionService.Convert(original.Value, original.Unit, LengthUnit.INCHES);
            var backToFeet = ConversionService.Convert(convertedToInches, LengthUnit.INCHES, LengthUnit.FEET);
            Assert.That(backToFeet, Is.EqualTo(original.Value).Within(1e-6));
        }
    }
}