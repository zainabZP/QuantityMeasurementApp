using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityLengthAdditionTests
    {
        [Test]
        public void TestAddition_SameUnit_FeetPlusFeet()
        {
            var l1 = new QuantityLength(1, LengthUnit.FEET);
            var l2 = new QuantityLength(2, LengthUnit.FEET);
            var sum = l1.Add(l2);
            Assert.That(sum.Value, Is.EqualTo(3.0));
            Assert.That(sum.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestAddition_CrossUnit_FeetPlusInches()
        {
            var l1 = new QuantityLength(1, LengthUnit.FEET);
            var l2 = new QuantityLength(12, LengthUnit.INCHES); // 12 inches = 1 foot
            var sum = l1.Add(l2);
            Assert.That(sum.Value, Is.EqualTo(2.0).Within(1e-6));
            Assert.That(sum.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestAddition_CrossUnit_InchesPlusFeet()
        {
            var l1 = new QuantityLength(12, LengthUnit.INCHES); // 12 inches = 1 foot
            var l2 = new QuantityLength(1, LengthUnit.FEET);
            var sum = l1.Add(l2);
            // sum is in first operand unit (INCHES)
            Assert.That(sum.Value, Is.EqualTo(24.0).Within(1e-6));
            Assert.That(sum.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void TestAddition_CrossUnit_YardsPlusFeet()
        {
            var l1 = new QuantityLength(1, LengthUnit.YARDS); // 1 yard = 3 feet
            var l2 = new QuantityLength(3, LengthUnit.FEET);  // 3 feet
            var sum = l1.Add(l2);
            // sum in first operand unit (YARDS): 1 yard + 3 feet = 2 yards
            Assert.That(sum.Value, Is.EqualTo(2.0).Within(1e-6));
            Assert.That(sum.Unit, Is.EqualTo(LengthUnit.YARDS));
        }

        [Test]
        public void TestAddition_CrossUnit_CentimetersPlusInch()
        {
            var l1 = new QuantityLength(2.54, LengthUnit.CENTIMETERS); // 2.54 cm = 1 inch
            var l2 = new QuantityLength(1, LengthUnit.INCHES);          // 1 inch
            var sum = l1.Add(l2);
            // sum in first operand unit (CENTIMETERS): 1 inch + 1 inch = 5.08 cm
            Assert.That(sum.Value, Is.EqualTo(5.08).Within(1e-2));
            Assert.That(sum.Unit, Is.EqualTo(LengthUnit.CENTIMETERS));
        }
    }
}