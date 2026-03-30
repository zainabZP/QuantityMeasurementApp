using NUnit.Framework;
using QM.Models.Models;
using QM.BusinessLogic.Service;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class UC14_TemperatureTests
    {
        private const double EPSILON = 0.01;

        // ============================
        // Temperature Equality Tests
        // ============================

        [Test]
        public void TemperatureEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void TemperatureEquality_CelsiusToFahrenheit_100Celsius212Fahrenheit()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void TemperatureEquality_CelsiusToKelvin_0Celsius273_15Kelvin()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void TemperatureEquality_NegativeValue_Minus40CelsiusMinus40Fahrenheit()
        {
            var t1 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.FAHRENHEIT);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void TemperatureEquality_ReflexiveProperty()
        {
            var t1 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            Assert.That(t1.Equals(t1), Is.True);
        }

        // ============================
        // Temperature Conversion Tests
        // ============================

        [Test]
        public void TemperatureConversion_CelsiusToFahrenheit_0Celsius()
        {
            var t = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var converted = t.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(converted.Value, Is.EqualTo(32.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(TemperatureUnit.FAHRENHEIT));
        }

        [Test]
        public void TemperatureConversion_CelsiusToFahrenheit_100Celsius()
        {
            var t = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var converted = t.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(converted.Value, Is.EqualTo(212.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(TemperatureUnit.FAHRENHEIT));
        }

        [Test]
        public void TemperatureConversion_CelsiusToKelvin_0Celsius()
        {
            var t = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var converted = t.ConvertTo(TemperatureUnit.KELVIN);

            Assert.That(converted.Value, Is.EqualTo(273.15).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(TemperatureUnit.KELVIN));
        }

        [Test]
        public void TemperatureConversion_FahrenheitToCelsius_32Fahrenheit()
        {
            var t = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            var converted = t.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.That(converted.Value, Is.EqualTo(0.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(TemperatureUnit.CELSIUS));
        }

        [Test]
        public void TemperatureConversion_RoundTrip_CelsiusToFahrenheitToCelsius()
        {
            var t = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            var toFahrenheit = t.ConvertTo(TemperatureUnit.FAHRENHEIT);
            var backToCelsius = toFahrenheit.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.That(backToCelsius.Value, Is.EqualTo(50.0).Within(EPSILON));
        }

        // ============================
        // Unsupported Operations Tests
        // ============================

        [Test]
        public void TemperatureUnsupportedOperation_AddThrowsException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);

            Assert.Throws<UnsupportedOperationException>(() => t1.Add(t2, TemperatureUnit.CELSIUS));
        }

        [Test]
        public void TemperatureUnsupportedOperation_SubtractThrowsException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);

            Assert.Throws<UnsupportedOperationException>(() => t1.Subtract(t2, TemperatureUnit.CELSIUS));
        }

        [Test]
        public void TemperatureUnsupportedOperation_DivideThrowsException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);

            Assert.Throws<UnsupportedOperationException>(() => t1.Divide(t2));
        }

        // ============================
        // Cross-Category Prevention Tests
        // ============================

        [Test]
        public void CrossCategory_TemperatureVsLength_ReturnsFalse()
        {
            var temp = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            var length = new Quantity<LengthUnit>(100.0, LengthUnit.FEET);

            Assert.That(temp.Equals(length), Is.False);
        }

        [Test]
        public void TemperatureValidateOperationSupport_ThrowsForAddition()
        {
            var celsius = TemperatureUnit.CELSIUS;
            Assert.Throws<UnsupportedOperationException>(() => celsius.ValidateOperationSupport("Add"));
        }
    }
}
