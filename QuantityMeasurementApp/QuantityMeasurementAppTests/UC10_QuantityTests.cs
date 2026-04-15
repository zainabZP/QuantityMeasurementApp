using NUnit.Framework;
using QM.Models.Models;
using QM.BusinessLogic.Service;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityTests
    {
        [Test]
        public void GivenFeetAndInches_ShouldReturnTrue()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2));
        }

        [Test]
        public void GivenKgAndGram_ShouldReturnTrue()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            Assert.That(w1.Equals(w2));
        }

        [Test]
        public void GivenAddition_ShouldReturnCorrectResult()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            var result = q1.Add(q2, LengthUnit.FEET);

            Assert.That(result.ToString(), Is.EqualTo("Quantity(2, FEET)"));
        }
    }
}