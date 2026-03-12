using NUnit.Framework;
using QuantityMeasurementApp;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class UC4Tests
    {
        [Test]
        public void TestEquality_YardToYard_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARDS);
            var q2 = new QuantityLength(1.0, LengthUnit.YARDS);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void TestEquality_YardToYard_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARDS);
            var q2 = new QuantityLength(2.0, LengthUnit.YARDS);

            Assert.That(q1.Equals(q2), Is.False);
        }

        [Test]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARDS);
            var feet = new QuantityLength(3.0, LengthUnit.FEET);

            Assert.That(yard.Equals(feet), Is.True);
        }

        [Test]
        public void TestEquality_YardToInches_EquivalentValue()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARDS);
            var inches = new QuantityLength(36.0, LengthUnit.INCHES);

            Assert.That(yard.Equals(inches), Is.True);
        }

        [Test]
        public void TestEquality_CentimeterToInches_ApproxEqual()
        {
            // 1 cm ≈ 0.0328084 ft → 0.393701 in
            var cm = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
            var inch = new QuantityLength(0.393701, LengthUnit.INCHES);

            Assert.That(cm.Equals(inch), Is.True);
        }

        [Test]
        public void TestEquality_CentimeterToFeet_NotEqual()
        {
            var cm = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
            var feet = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.That(cm.Equals(feet), Is.False);
        }

        [Test]
        public void TestEquality_AllUnits_Transitive()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARDS);
            var feet = new QuantityLength(3.0, LengthUnit.FEET);
            var inches = new QuantityLength(36.0, LengthUnit.INCHES);

            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inches), Is.True);
            Assert.That(yard.Equals(inches), Is.True);
        }

        [Test]
        public void TestEquality_SameReference()
        {
            var q = new QuantityLength(2.0, LengthUnit.YARDS);

            Assert.That(q.Equals(q), Is.True);
        }

        [Test]
        public void TestEquality_WithNull()
        {
            var q = new QuantityLength(2.0, LengthUnit.YARDS);

            Assert.That(q.Equals(null), Is.False);
        }

        [Test]
        public void TestEquality_DifferentUnits_NotEqual()
        {
            var yard = new QuantityLength(1.0, LengthUnit.YARDS);
            var inch = new QuantityLength(10.0, LengthUnit.INCHES);

            Assert.That(yard.Equals(inch), Is.False);
        }

        [Test]
        public void TestEquality_ComplexScenario()
        {
            var yard = new QuantityLength(2.0, LengthUnit.YARDS);
            var feet = new QuantityLength(6.0, LengthUnit.FEET);
            var inches = new QuantityLength(72.0, LengthUnit.INCHES);

            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inches), Is.True);
            Assert.That(yard.Equals(inches), Is.True);
        }
    }
}