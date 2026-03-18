using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    public class WeightTests
    {
        private WeightService weightService = new WeightService();

        [Test]
        public void GivenSameWeight_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(1000, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1, WeightUnit.KILOGRAM);

            Assert.That(weightService.AreEqual(w1, w2), Is.True);
        }

        [Test]
        public void GivenDifferentWeight_ShouldReturnFalse()
        {
            QuantityWeight w1 = new QuantityWeight(1000, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(2, WeightUnit.KILOGRAM);

            Assert.That(weightService.AreEqual(w1, w2), Is.False);
        }

        [Test]
        public void GivenPoundAndGram_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(453.592, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1, WeightUnit.POUND);

            Assert.That(weightService.AreEqual(w1, w2), Is.True);
        }
    }
}