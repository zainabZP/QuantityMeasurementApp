using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class UC7Tests
    {
        private const double EPSILON = 0.001;
        private LengthService lengthService;

        [SetUp]
        public void Setup()
        {
            lengthService = new LengthService();
        }

        [Test]
        public void TestAddition_TargetUnit_Feet()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestAddition_TargetUnit_Inches()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.INCHES);

            Assert.That(result.Value, Is.EqualTo(24.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void TestAddition_TargetUnit_Yards()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.YARDS);

            Assert.That(result.Value, Is.EqualTo(0.666).Within(0.01));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.YARDS));
        }

        [Test]
        public void TestAddition_TargetUnit_Centimeters()
        {
            var a = new QuantityLength(1.0, LengthUnit.INCHES);
            var b = new QuantityLength(1.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.CENTIMETERS);

            Assert.That(result.Value, Is.EqualTo(5.08).Within(0.01));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.CENTIMETERS));
        }

        [Test]
        public void TestAddition_SameAsFirstOperandUnit()
        {
            var a = new QuantityLength(2.0, LengthUnit.YARDS);
            var b = new QuantityLength(3.0, LengthUnit.FEET);

            var result = lengthService.AddLengths(a, b, LengthUnit.YARDS);

            Assert.That(result.Value, Is.EqualTo(3.0).Within(EPSILON));
        }

        [Test]
        public void TestAddition_SameAsSecondOperandUnit()
        {
            var a = new QuantityLength(2.0, LengthUnit.YARDS);
            var b = new QuantityLength(3.0, LengthUnit.FEET);

            var result = lengthService.AddLengths(a, b, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(9.0).Within(EPSILON));
        }

        [Test]
        public void TestAddition_Commutativity()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result1 = lengthService.AddLengths(a, b, LengthUnit.YARDS);
            var result2 = lengthService.AddLengths(b, a, LengthUnit.YARDS);

            Assert.That(result1.Value, Is.EqualTo(result2.Value).Within(EPSILON));
        }

        [Test]
        public void TestAddition_WithZero()
        {
            var a = new QuantityLength(5.0, LengthUnit.FEET);
            var b = new QuantityLength(0.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.YARDS);

            Assert.That(result.Value, Is.EqualTo(1.667).Within(0.01));
        }

        [Test]
        public void TestAddition_NegativeValues()
        {
            var a = new QuantityLength(5.0, LengthUnit.FEET);
            var b = new QuantityLength(-2.0, LengthUnit.FEET);

            var result = lengthService.AddLengths(a, b, LengthUnit.INCHES);

            Assert.That(result.Value, Is.EqualTo(36.0).Within(EPSILON));
        }

        [Test]
        public void TestAddition_InvalidTargetUnit_ShouldThrowException()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            Assert.Throws<InvalidOperationException>(() =>
                lengthService.AddLengths(a, b, (LengthUnit)999));
        }

        [Test]
        public void TestAddition_LargeToSmallScale()
        {
            var a = new QuantityLength(1000.0, LengthUnit.FEET);
            var b = new QuantityLength(500.0, LengthUnit.FEET);

            var result = lengthService.AddLengths(a, b, LengthUnit.INCHES);

            Assert.That(result.Value, Is.EqualTo(18000.0).Within(EPSILON));
        }

        [Test]
        public void TestAddition_SmallToLargeScale()
        {
            var a = new QuantityLength(12.0, LengthUnit.INCHES);
            var b = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = lengthService.AddLengths(a, b, LengthUnit.YARDS);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(0.01));
        }
    }
}