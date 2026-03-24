using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QM.Models.DTOs;
using QM.Models.Entities;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;

namespace QuantityMeasurementAppTests.Integration
{
    [TestFixture]
    public class QuantityMeasurementIntegrationTests
    {
        private QuantityMeasurementDbContext _context;
        private IQuantityMeasurementRepository _repository;
        private IQuantityMeasurementService _service;

        [SetUp]
        public void Setup()
        {
            // Setup dependency injection with in-memory database
            var services = new ServiceCollection();
            
            var options = new DbContextOptionsBuilder<QuantityMeasurementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new QuantityMeasurementDbContext(options);
            
            services.AddScoped<QuantityMeasurementDbContext>(_ => _context);
            services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
            services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

            var serviceProvider = services.BuildServiceProvider();
            _repository = serviceProvider.GetRequiredService<IQuantityMeasurementRepository>();
            _service = serviceProvider.GetRequiredService<IQuantityMeasurementService>();
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public void EndToEnd_ComparisonOperation_SavesAndRetrievesCorrectly()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(12, "INCHES", "Length");

            // Act
            var result = _service.Compare(q1, q2);
            var allMeasurements = _repository.GetAll();

            // Assert
            Assert.That(result.Value, Is.EqualTo(1)); // Equal
            Assert.That(allMeasurements.Count, Is.EqualTo(1));
            Assert.That(allMeasurements[0].OperationType, Is.EqualTo("Compare"));
        }

        [Test]
        public void EndToEnd_ConversionOperation_SavesAndRetrievesCorrectly()
        {
            // Arrange
            var source = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Convert(source, "INCHES");
            var allMeasurements = _repository.GetAll();

            // Assert
            Assert.That(result.Value, Is.EqualTo(12));
            Assert.That(allMeasurements.Count, Is.EqualTo(1));
            Assert.That(allMeasurements[0].OperationType, Is.EqualTo("Convert"));
        }

        [Test]
        public void EndToEnd_AddOperation_SavesAndRetrievesCorrectly()
        {
            // Arrange
            var q1 = new QuantityDTO(1, "FEET", "Length");
            var q2 = new QuantityDTO(1, "FEET", "Length");

            // Act
            var result = _service.Add(q1, q2, "FEET");
            var allMeasurements = _repository.GetAll();

            // Assert
            Assert.That(result.Value, Is.EqualTo(2));
            Assert.That(allMeasurements.Count, Is.EqualTo(1));
            Assert.That(allMeasurements[0].OperationType, Is.EqualTo("Add"));
        }

        [Test]
        public void EndToEnd_MultipleOperations_AllSaved()
        {
            // Arrange & Act
            _service.Compare(new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            _service.Convert(new QuantityDTO(1, "FEET", "Length"), "INCHES");
            _service.Add(new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(1, "FEET", "Length"), "FEET");
            _service.Divide(new QuantityDTO(2, "FEET", "Length"), new QuantityDTO(1, "FEET", "Length"));

            var allMeasurements = _repository.GetAll();

            // Assert
            Assert.That(allMeasurements.Count, Is.EqualTo(4));
            Assert.That(allMeasurements.Any(m => m.OperationType == "Compare"), Is.True);
            Assert.That(allMeasurements.Any(m => m.OperationType == "Convert"), Is.True);
            Assert.That(allMeasurements.Any(m => m.OperationType == "Add"), Is.True);
            Assert.That(allMeasurements.Any(m => m.OperationType == "Divide"), Is.True);
        }

        [Test]
        public void EndToEnd_QueryByOperationType_ReturnsCorrectResults()
        {
            // Arrange & Act
            _service.Compare(new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            _service.Compare(new QuantityDTO(2, "FEET", "Length"), new QuantityDTO(2, "FEET", "Length"));
            _service.Convert(new QuantityDTO(1, "FEET", "Length"), "INCHES");

            var compareOps = _repository.GetByOperationType("Compare");

            // Assert
            Assert.That(compareOps.Count, Is.EqualTo(2));
            Assert.That(compareOps.All(m => m.OperationType == "Compare"), Is.True);
        }

        [Test]
        public void EndToEnd_ClearRepository_RemovesAllData()
        {
            // Arrange & Act
            _service.Compare(new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            _service.Add(new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(1, "FEET", "Length"), "FEET");
            
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(2));
            
            _repository.Clear();

            // Assert
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(0));
            Assert.That(_repository.GetAll().Count, Is.EqualTo(0));
        }

        [Test]
        public void EndToEnd_WeightOperations_SavesWithCorrectMeasurementType()
        {
            // Arrange & Act
            var q1 = new QuantityDTO(5, "KG", "Weight");
            var q2 = new QuantityDTO(5000, "GRAM", "Weight");
            
            _service.Compare(q1, q2);
            
            var measurements = _repository.GetAll();

            // Assert
            Assert.That(measurements.Count, Is.EqualTo(1));
            Assert.That((measurements[0].Operand1 ?? "").Contains("Weight"));
        }

        [Test]
        public void EndToEnd_VolumeOperations_SavesWithCorrectMeasurementType()
        {
            // Arrange & Act
            var q1 = new QuantityDTO(1, "LITRE", "Volume");
            var q2 = new QuantityDTO(1000, "ML", "Volume");
            
            _service.Compare(q1, q2);
            
            var measurements = _repository.GetAll();

            // Assert
            Assert.That(measurements.Count, Is.EqualTo(1));
            Assert.That((measurements[0].Operand1 ?? "").Contains("Volume"));
        }

        [Test]
        public void EndToEnd_DatabasePersistence_DataSurvivesRepositoryRecreation()
        {
            // Arrange & Act
            var q1 = new QuantityDTO(1, "FEET", "Length");
            const string targetUnit = "INCHES";
            
            // First service instance
            _service.Convert(q1, targetUnit);
            var countAfterFirstSave = _repository.GetTotalCount();

            // Create new repository (simulating app restart)
            var newRepository = new QuantityMeasurementDatabaseRepository(_context);
            var countAfterReload = newRepository.GetTotalCount();

            // Assert
            Assert.That(countAfterFirstSave, Is.EqualTo(1));
            Assert.That(countAfterReload, Is.EqualTo(1)); // Data persisted
        }
    }
}
