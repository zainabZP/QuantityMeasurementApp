using NUnit.Framework;
using QM.Models.Models;
using QM.Models.DTOs;
using QM.BusinessLogic.Service;
using QM.BusinessLogic.Interface;
using QuantityMeasurementApp.Controllers;
using QM.Repository.Repository;
using QM.Repository.Interface;
using System;

namespace QuantityMeasurementAppTests
{
    [TestFixture]
    public class UC16_BackwardCompatibility_CacheRepositoryTests
    {
        private QuantityMeasurementController _controller;
        private IQuantityMeasurementRepository _repository;
        private const double EPSILON = 0.001;

        [SetUp]
        public void Setup()
        {
            // Use cache repository for backward compatibility tests
            _repository = QuantityMeasurementCacheRepository.Instance;
            var service = new QuantityMeasurementServiceImpl(_repository);
            _controller = new QuantityMeasurementController(service);
            
            // Clear repository for each test
            _repository.Clear();
        }

        #region Compare Tests

        [Test]
        public void TestCompare_FeetEquality_SameValue()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformCompare(q1, q2);

            Assert.That(result.Value, Is.EqualTo(1)); // Equal
        }

        [Test]
        public void TestCompare_FeetToInches_EquivalentValue()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            var result = _controller.PerformCompare(q1, q2);

            Assert.That(result.Value, Is.EqualTo(1)); // Equal
        }

        [Test]
        public void TestCompare_FeetInequality()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(2, "FEET", "Length");

            var result = _controller.PerformCompare(q1, q2);

            Assert.That(result.Value, Is.EqualTo(0)); // Not equal
        }

        [Test]
        public void TestCompare_YardToFeetEquivalent()
        {
            var q1 = new QuantityDTO(1, "YARD", "Length");
            var q2 = new QuantityDTO(3, "FEET", "Length");

            var result = _controller.PerformCompare(q1, q2);

            Assert.That(result.Value, Is.EqualTo(1)); // Equal
        }

        [Test]
        public void TestCompare_CentimeterToInches()
        {
            var q1 = new QuantityDTO(1, "CM", "Length");
            var q2 = new QuantityDTO(0.393701, "INCHES", "Length");

            var result = _controller.PerformCompare(q1, q2);

            Assert.That(result.Value, Is.EqualTo(1)); // Equal (within tolerance)
        }

        #endregion

        #region Convert Tests

        [Test]
        public void TestConvert_FeetToInches()
        {
            var source = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformConvert(source, "INCHES");

            Assert.That(result.Value, Is.EqualTo(12).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo("INCHES"));
        }

        [Test]
        public void TestConvert_InchesToFeet()
        {
            var source = new QuantityDTO(12, "INCHES", "Length");

            var result = _controller.PerformConvert(source, "FEET");

            Assert.That(result.Value, Is.EqualTo(1).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo("FEET"));
        }

        [Test]
        public void TestConvert_KilogramToGram()
        {
            var source = new QuantityDTO(1, "KG", "Weight");

            var result = _controller.PerformConvert(source, "GRAM");

            Assert.That(result.Value, Is.EqualTo(1000).Within(EPSILON));
        }

        [Test]
        public void TestConvert_LitreToMillilitre()
        {
            var source = new QuantityDTO(1, "LITRE", "Volume");

            var result = _controller.PerformConvert(source, "ML");

            Assert.That(result.Value, Is.EqualTo(1000).Within(EPSILON));
        }

        #endregion

        #region Add Tests

        [Test]
        public void TestAdd_FeetPlusFeet()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformAdd(q1, q2, "FEET");

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        [Test]
        public void TestAdd_FeetPlusInches()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            var result = _controller.PerformAdd(q1, q2, "FEET");

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        [Test]
        public void TestAdd_KilogramPlusGram()
        {
            var q1 = new QuantityDTO(1, "KG", "Weight");
            var q2 = new QuantityDTO(1000, "GRAM", "Weight");

            var result = _controller.PerformAdd(q1, q2, "KG");

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        #endregion

        #region Subtract Tests

        [Test]
        public void TestSubtract_FeetMinusFeet()
        {
            var q1 = new QuantityDTO(2, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformSubtract(q1, q2, "FEET");

            Assert.That(result.Value, Is.EqualTo(1).Within(EPSILON));
        }

        [Test]
        public void TestSubtract_FeetMinusInches()
        {
            var q1 = new QuantityDTO(2, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            var result = _controller.PerformSubtract(q1, q2, "FEET");

            Assert.That(result.Value, Is.EqualTo(1).Within(EPSILON));
        }

        [Test]
        public void TestSubtract_KilogramMinusGram()
        {
            var q1 = new QuantityDTO(2, "KG", "Weight");
            var q2 = new QuantityDTO(1000, "GRAM", "Weight");

            var result = _controller.PerformSubtract(q1, q2, "KG");

            Assert.That(result.Value, Is.EqualTo(1).Within(EPSILON));
        }

        #endregion

        #region Divide Tests

        [Test]
        public void TestDivide_FeetDividedByFeet()
        {
            var q1 = new QuantityDTO(2, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformDivide(q1, q2);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        [Test]
        public void TestDivide_InchesAndFeet()
        {
            var q1 = new QuantityDTO(24, "INCHES", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            var result = _controller.PerformDivide(q1, q2);

            Assert.That(result.Value, Is.EqualTo(2).Within(EPSILON));
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public void TestCompare_DifferentCategories_ThrowsException()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "KG", "Weight");

            Assert.Throws<QM.Models.Exceptions.QuantityMeasurementException>(
                () => _controller.PerformCompare(q1, q2));
        }

        [Test]
        public void TestAdd_Temperature_ThrowsException()
        {
            var q1 = new QuantityDTO(10, "CELSIUS", "Temperature");
            var q2 = new QuantityDTO(20, "CELSIUS", "Temperature");

            Assert.Throws<QM.Models.Exceptions.QuantityMeasurementException>(
                () => _controller.PerformAdd(q1, q2, "CELSIUS"));
        }

        [Test]
        public void TestDivide_ByZero_ThrowsException()
        {
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(0, "FEET", "Length");

            Assert.Throws<QM.Models.Exceptions.QuantityMeasurementException>(
                () => _controller.PerformDivide(q1, q2));
        }

        #endregion

        [TearDown]
        public void TearDown()
        {
            _repository.Clear();
        }
    }
}
