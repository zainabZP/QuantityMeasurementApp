using NUnit.Framework;
using Moq;
using QM.Models.DTOs;
using QM.Models.Exceptions;
using QM.Repository.Interface;
using QM.BusinessLogic.Service;

namespace QuantityMeasurementAppTests.Service
{
    [TestFixture]
    public class QuantityMeasurementServiceTests
    {
        private Mock<IQuantityMeasurementRepository> _repositoryMock;
        private QuantityMeasurementServiceImpl _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IQuantityMeasurementRepository>();
            _service = new QuantityMeasurementServiceImpl(_repositoryMock.Object);
        }

        [Test]
        public void Compare_SameValues_ReturnsEqual()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            // Act
            var result = _service.Compare(q1, q2);

            // Assert
            Assert.That(result.Value, Is.EqualTo(1)); // Equal
            _repositoryMock.Verify(r => r.Save(It.IsAny<QM.Models.Entities.QuantityMeasurementEntity>()), Times.Once);
        }

        [Test]
        public void Compare_DifferentValues_ReturnsNotEqual()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(2, "FEET", "Length");

            // Act
            var result = _service.Compare(q1, q2);

            // Assert
            Assert.That(result.Value, Is.EqualTo(0)); // Not equal
        }

        [Test]
        public void Compare_DifferentCategories_ThrowsException()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "KG", "Weight");

            // Act & Assert
            Assert.Throws<QuantityMeasurementException>(() => _service.Compare(q1, q2));
        }

        [Test]
        public void Convert_ValidConversion_ReturnsCorrectValue()
        {
            // Arrange
            var source = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Convert(source, "INCHES");

            // Assert
            Assert.That(result.Value, Is.EqualTo(12));
            Assert.That(result.Unit, Is.EqualTo("INCHES"));
            _repositoryMock.Verify(r => r.Save(It.IsAny<QM.Models.Entities.QuantityMeasurementEntity>()), Times.Once);
        }

        [Test]
        public void Add_ValidAddition_ReturnsCorrectSum()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Add(q1, q2, "FEET");

            // Assert
            Assert.That(result.Value, Is.EqualTo(2));
            Assert.That(result.Unit, Is.EqualTo("FEET"));
        }

        [Test]
        public void Add_DifferentUnits_ReturnsCorrectSum()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            // Act
            var result = _service.Add(q1, q2, "FEET");

            // Assert
            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void Subtract_ValidSubtraction_ReturnsCorrectDifference()
        {
            // Arrange
            var q1 = new QuantityDTO(2, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Subtract(q1, q2, "FEET");

            // Assert
            Assert.That(result.Value, Is.EqualTo(1));
        }

        [Test]
        public void Divide_ValidDivision_ReturnsCorrectQuotient()
        {
            // Arrange
            var q1 = new QuantityDTO(2, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Divide(q1, q2);

            // Assert
            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void Divide_DivideByZero_ThrowsException()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(0, "FEET", "Length");

            // Act & Assert
            Assert.Throws<QuantityMeasurementException>(() => _service.Divide(q1, q2));
        }

        [Test]
        public void Add_TemperatureType_ThrowsException()
        {
            // Arrange
            var q1 = new QuantityDTO(10, "CELSIUS", "Temperature");
            var q2 = new QuantityDTO(20, "CELSIUS", "Temperature");

            // Act & Assert
            Assert.Throws<QuantityMeasurementException>(() => _service.Add(q1, q2, "CELSIUS"));
        }

        [Test]
        public void Compare_NullFirstQuantity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<QuantityMeasurementException>(() => _service.Compare(null!, new QuantityDTO(1, "FEET", "Length")));
        }
    }
}
