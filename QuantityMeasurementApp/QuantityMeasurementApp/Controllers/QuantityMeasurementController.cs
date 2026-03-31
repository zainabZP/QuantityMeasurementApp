using Microsoft.Extensions.Logging;
using QM.Models.DTOs;
using QM.BusinessLogic.Interface;

namespace QuantityMeasurementApp.Controllers
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;
        private readonly ILogger<QuantityMeasurementController>? _logger;

        public QuantityMeasurementController(IQuantityMeasurementService service, 
            ILogger<QuantityMeasurementController>? logger = null)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
            _logger?.LogInformation("Controller initialized");
        }

        public QuantityDTO PerformCompare(QuantityDTO q1, QuantityDTO q2)
        {
            _logger?.LogDebug("Performing compare operation");
            return _service.Compare(q1, q2);
        }

        public QuantityDTO PerformConvert(QuantityDTO source, string targetUnit)
        {
            _logger?.LogDebug("Performing convert operation");
            return _service.Convert(source, targetUnit);
        }

        public QuantityDTO PerformAdd(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            _logger?.LogDebug("Performing add operation");
            return _service.Add(q1, q2, targetUnit);
        }

        public QuantityDTO PerformSubtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            _logger?.LogDebug("Performing subtract operation");
            return _service.Subtract(q1, q2, targetUnit);
        }

        public QuantityDTO PerformDivide(QuantityDTO q1, QuantityDTO q2)
        {
            _logger?.LogDebug("Performing divide operation");
            return _service.Divide(q1, q2);
        }
    }
}
