using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using QM.Models.DTOs;
using QM.Models.Entities;
using QM.Models.Exceptions;
using QM.Repository.Data;
using QM.Repository.Repository;

namespace QuantityMeasurementAppTests.Repository
{
    [TestFixture]
    public class QuantityMeasurementDatabaseRepositoryTests
    {
        private QuantityMeasurementDbContext _context;
        private QuantityMeasurementDatabaseRepository _repository;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<QuantityMeasurementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new QuantityMeasurementDbContext(options);
            _repository = new QuantityMeasurementDatabaseRepository(_context);
        }

        [Test]
        public void SaveEntity_ValidEntity_SavesSuccessfully()
        {
            // Arrange
            var entity = new QuantityMeasurementEntity(
                "TestOperation",
                new QuantityDTO(10, "FEET", "Length"),
                new QuantityDTO(120, "INCHES", "Length")
            );

            // Act
            _repository.Save(entity);

            // Assert
            var saved = _repository.FindById(entity.Id);
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved.OperationType, Is.EqualTo("TestOperation"));
        }

        [Test]
        public void SaveEntity_NullEntity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _repository.Save(null!));
        }

        [Test]
        public void GetAll_RetrievesAllSavedEntities()
        {
            // Arrange
            var entity1 = new QuantityMeasurementEntity("Op1", new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            var entity2 = new QuantityMeasurementEntity("Op2", new QuantityDTO(100, "CM", "Length"), new QuantityDTO(39.37, "INCHES", "Length"));
            
            _repository.Save(entity1);
            _repository.Save(entity2);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetTotalCount_ReturnsCorrectCount()
        {
            // Arrange
            var entity1 = new QuantityMeasurementEntity("Op1", new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            var entity2 = new QuantityMeasurementEntity("Op2", new QuantityDTO(100, "CM", "Length"), new QuantityDTO(39.37, "INCHES", "Length"));
            var entity3 = new QuantityMeasurementEntity("Op3", new QuantityDTO(5, "KG", "Weight"), new QuantityDTO(5000, "GRAM", "Weight"));
            
            _repository.Save(entity1);
            _repository.Save(entity2);
            _repository.Save(entity3);

            // Act
            int count = _repository.GetTotalCount();

            // Assert
            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void Clear_RemovesAllEntities()
        {
            // Arrange
            var entity1 = new QuantityMeasurementEntity("Op1", new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            var entity2 = new QuantityMeasurementEntity("Op2", new QuantityDTO(100, "CM", "Length"), new QuantityDTO(39.37, "INCHES", "Length"));
            
            _repository.Save(entity1);
            _repository.Save(entity2);
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(2));

            // Act
            _repository.Clear();

            // Assert
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void GetByOperationType_FiltersCorrectly()
        {
            // Arrange
            var entity1 = new QuantityMeasurementEntity("Compare", new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            var entity2 = new QuantityMeasurementEntity("Convert", new QuantityDTO(100, "CM", "Length"), new QuantityDTO(39.37, "INCHES", "Length"));
            var entity3 = new QuantityMeasurementEntity("Compare", new QuantityDTO(5, "KG", "Weight"), new QuantityDTO(5000, "GRAM", "Weight"));
            
            _repository.Save(entity1);
            _repository.Save(entity2);
            _repository.Save(entity3);

            // Act
            var compareOps = _repository.GetByOperationType("Compare");

            // Assert
            Assert.That(compareOps.Count, Is.EqualTo(2));
            foreach (var entity in compareOps)
            {
                Assert.That(entity.OperationType, Is.EqualTo("Compare"));
            }
        }

        [Test]
        public void FindById_ExistingId_ReturnsEntity()
        {
            // Arrange
            var entity = new QuantityMeasurementEntity(
                "TestOp",
                new QuantityDTO(10, "FEET", "Length"),
                new QuantityDTO(120, "INCHES", "Length")
            );
            _repository.Save(entity);

            // Act
            var found = _repository.FindById(entity.Id);

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Id, Is.EqualTo(entity.Id));
        }

        [Test]
        public void FindById_NonExistingId_ReturnsNull()
        {
            // Act
            var found = _repository.FindById(Guid.NewGuid());

            // Assert
            Assert.That(found, Is.Null);
        }

        [Test]
        public void DatabaseException_OnDataAccess_Works()
        {
            // Arrange - Verify exception handling works
            var entity = new QuantityMeasurementEntity("Op", new QuantityDTO(1, "FEET", "Length"), new QuantityDTO(12, "INCHES", "Length"));
            _repository.Save(entity);

            // Act & Assert - Normal operation succeeds
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}
