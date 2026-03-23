using QM.Models.Models;
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
    }
}
