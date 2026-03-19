using QM.Models.DTOs;
using QM.BusinessLogic.Interface;

namespace QuantityMeasurementApp.Controllers
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public QuantityDTO PerformCompare(QuantityDTO q1, QuantityDTO q2)
        {
            return _service.Compare(q1, q2);
        }

        public QuantityDTO PerformConvert(QuantityDTO source, string targetUnit)
        {
            return _service.Convert(source, targetUnit);
        }

        public QuantityDTO PerformAdd(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            return _service.Add(q1, q2, targetUnit);
        }

        public QuantityDTO PerformSubtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            return _service.Subtract(q1, q2, targetUnit);
        }

        public QuantityDTO PerformDivide(QuantityDTO q1, QuantityDTO q2)
        {
            return _service.Divide(q1, q2);
        }
    }
}