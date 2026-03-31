using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QM.Models.Entities;
using QM.Models.Exceptions;
using QM.Repository.Data;
using QM.Repository.Interface;

namespace QM.Repository.Repository
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _context;
        private readonly ILogger<QuantityMeasurementDatabaseRepository>? _logger;

        public QuantityMeasurementDatabaseRepository(QuantityMeasurementDbContext context, 
            ILogger<QuantityMeasurementDatabaseRepository>? logger = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _logger?.LogInformation("Database Repository initialized");
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            try
            {
                _context.Measurements.Add(entity);
                _context.SaveChanges();
                _logger?.LogDebug($"Entity saved to database. ID: {entity.Id}");
            }
            catch (DbUpdateException dbEx)
            {
                _logger?.LogError($"Database error while saving: {dbEx.Message}");
                throw new DatabaseException("Failed to save measurement to database", dbEx);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Unexpected error while saving: {ex.Message}");
                throw new DatabaseException("Unexpected error while saving measurement", ex);
            }
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            try
            {
                var measurements = _context.Measurements.OrderByDescending(m => m.Timestamp).ToList();
                _logger?.LogDebug($"Retrieved {measurements.Count} measurements from database");
                return measurements;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error retrieving all measurements: {ex.Message}");
                throw new DatabaseException("Failed to retrieve measurements from database", ex);
            }
        }

        public QuantityMeasurementEntity? FindById(Guid id)
        {
            try
            {
                return _context.Measurements.FirstOrDefault(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error finding measurement by ID: {ex.Message}");
                throw new DatabaseException("Failed to find measurement by ID", ex);
            }
        }

        public void Clear()
        {
            try
            {
                _context.Measurements.RemoveRange(_context.Measurements);
                _context.SaveChanges();
                _logger?.LogInformation("All measurements cleared from database");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error clearing measurements: {ex.Message}");
                throw new DatabaseException("Failed to clear measurements", ex);
            }
        }

        public int GetTotalCount()
        {
            try
            {
                return _context.Measurements.Count();
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting total count: {ex.Message}");
                throw new DatabaseException("Failed to get measurement count", ex);
            }
        }

        public List<QuantityMeasurementEntity> GetByOperationType(string operationType)
        {
            try
            {
                return _context.Measurements
                    .Where(m => m.OperationType.ToUpper() == operationType.ToUpper())
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting measurements by operation type: {ex.Message}");
                throw new DatabaseException($"Failed to get measurements by operation type: {operationType}", ex);
            }
        }

        public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
        {
            try
            {
                return _context.Measurements
                    .Where(m => (m.Operand1 != null && m.Operand1.Contains(measurementType)) ||
                               (m.Result != null && m.Result.Contains(measurementType)))
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting measurements by measurement type: {ex.Message}");
                throw new DatabaseException($"Failed to get measurements by measurement type: {measurementType}", ex);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            _logger?.LogInformation("Database Repository disposed");
        }

        // Add these two methods — pure EF Core ORM LINQ, no raw SQL
        public List<QuantityMeasurementEntity> GetErroredMeasurements()
        {
            try
            {
                return _context.Measurements
                    .Where(m => m.HasError == true)
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting errored measurements: {ex.Message}");
                throw new DatabaseException("Failed to get errored measurements", ex);
            }
        }

        public int GetCountByOperationType(string operationType)
        {
            try
            {
                return _context.Measurements
                    .Count(m => m.OperationType.ToUpper() == operationType.ToUpper());
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting count by operation type: {ex.Message}");
                throw new DatabaseException($"Failed to get count for operation type: {operationType}", ex);
            }
        }
    }
}
