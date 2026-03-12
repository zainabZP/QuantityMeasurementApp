using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityLengthTests
    {
        // Feet to Feet Same Value
        [Test]
        public void TestEquality_FeetToFeet_SameValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.That(a.Equals(b), Is.True);
        }

        // Inch to Inch Same Value
        [Test]
        public void TestEquality_InchToInch_SameValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.INCHES);
            var b = new QuantityLength(1.0, LengthUnit.INCHES);

            Assert.That(a.Equals(b), Is.True);
        }

        // Feet to Inches Equivalent (1 ft = 12 in)
        [Test]
        public void TestEquality_FeetToInch_EquivalentValue()
        {
            var feet = new QuantityLength(1.0, LengthUnit.FEET);
            var inches = new QuantityLength(12.0, LengthUnit.INCHES);

            Assert.That(feet.Equals(inches), Is.True);
        }

        // Inches to Feet Equivalent (Symmetry)
        [Test]
        public void TestEquality_InchToFeet_EquivalentValue()
        {
            var inches = new QuantityLength(12.0, LengthUnit.INCHES);
            var feet = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.That(inches.Equals(feet), Is.True);
        }

        // Feet Different Values
        [Test]
        public void TestEquality_FeetToFeet_DifferentValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);
            var b = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.That(a.Equals(b), Is.False);
        }

        // Inches Different Values
        [Test]
        public void TestEquality_InchToInch_DifferentValue()
        {
            var a = new QuantityLength(1.0, LengthUnit.INCHES);
            var b = new QuantityLength(2.0, LengthUnit.INCHES);

            Assert.That(a.Equals(b), Is.False);
        }

        // Cross Unit Inequality (1 ft ≠ 1 inch)
        [Test]
        public void TestEquality_CrossUnit_Inequality()
        {
            var feet = new QuantityLength(1.0, LengthUnit.FEET);
            var inch = new QuantityLength(1.0, LengthUnit.INCHES);

            Assert.That(feet.Equals(inch), Is.False);
        }

        // Same Reference (Reflexive Property)
        [Test]
        public void TestEquality_SameReference()
        {
            var a = new QuantityLength(5.0, LengthUnit.FEET);

            Assert.That(a.Equals(a), Is.True);
        }

        // Null Comparison
        [Test]
        public void TestEquality_NullComparison()
        {
            var a = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.That(a.Equals(null), Is.False);
        }

        // Multiple Comparison Consistency
        [Test]
        public void TestEquality_MultipleFeetComparison()
        {
            var a = new QuantityLength(3.0, LengthUnit.FEET);
            var b = new QuantityLength(36.0, LengthUnit.INCHES);
            var c = new QuantityLength(3.0, LengthUnit.FEET);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(b.Equals(c), Is.True);
            Assert.That(a.Equals(c), Is.True);
        }
    }
}