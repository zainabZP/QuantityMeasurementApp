using QM.Models.DTOs;
using QM.Repository.Interface; 

namespace QM.BusinessLogic.Interface
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Convert(QuantityDTO source, string targetUnit);
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit);
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit);
        QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2);
    }
}