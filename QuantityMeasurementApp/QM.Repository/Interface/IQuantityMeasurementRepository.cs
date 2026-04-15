using QM.Models.DTOs;
using QM.Models.Entities;

namespace QM.Repository.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll(string? userId = null);
        QuantityMeasurementEntity? FindById(Guid id);
        void Clear();
        int GetTotalCount(string? userId = null);
        List<QuantityMeasurementEntity> GetByOperationType(string operationType, string? userId = null);
        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType, string? userId = null);

        // UC17 new methods (EF Core ORM LINQ — equivalent of Spring Data JPA query methods)
        List<QuantityMeasurementEntity> GetErroredMeasurements(string? userId = null);
        int GetCountByOperationType(string operationType, string? userId = null);
    }
}