using QM.Models.DTOs;
using QM.Models.Entities;

namespace QM.Repository.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
        QuantityMeasurementEntity? FindById(Guid id);
        void Clear();
        int GetTotalCount();
        List<QuantityMeasurementEntity> GetByOperationType(string operationType);
        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);

        // UC17 new methods (EF Core ORM LINQ — equivalent of Spring Data JPA query methods)
        List<QuantityMeasurementEntity> GetErroredMeasurements();
        int GetCountByOperationType(string operationType);
    }
}