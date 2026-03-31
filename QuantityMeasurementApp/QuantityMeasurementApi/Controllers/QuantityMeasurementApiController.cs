using Microsoft.AspNetCore.Mvc;
using QM.BusinessLogic.Interface;
using QM.Models.DTOs;
using QM.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace QuantityMeasurementApi.Controllers
{
    /// <summary>
    /// REST API for Quantity Measurement operations
    /// </summary>
    [Authorize]    
    [ApiController]
    [Route("api/v1/quantities")]
    [Tags("Quantity Measurements")]
    [Produces("application/json")]
    public class QuantityMeasurementApiController : ControllerBase
    {
        private readonly IQuantityMeasurementService    _service;
        private readonly IQuantityMeasurementRepository _repository;
        private readonly ILogger<QuantityMeasurementApiController> _logger;

        public QuantityMeasurementApiController(
            IQuantityMeasurementService    service,
            IQuantityMeasurementRepository repository,
            ILogger<QuantityMeasurementApiController> logger)
        {
            _service    = service;
            _repository = repository;
            _logger     = logger;
        }

        /// <summary>Compare two quantities — returns 1 if equal, 0 if not equal</summary>
        [HttpPost("compare")]
        [SwaggerOperation(Summary = "Compare two quantities", Description = "Returns 1 if equal, 0 if not")]
        [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /compare called");
            var result = _service.Compare(input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(result);
        }

        /// <summary>Convert a quantity to a target unit</summary>
        [HttpPost("convert")]
        [SwaggerOperation(Summary = "Convert quantity to target unit")]
        [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /convert called");
            var result = _service.Convert(input.ThisQuantityDTO, input.ThatQuantityDTO.Unit);
            return Ok(result);
        }

        /// <summary>Add two quantities</summary>
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add two quantities")]
        [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /add called");
            var result = _service.Add(
                input.ThisQuantityDTO,
                input.ThatQuantityDTO,
                input.ThatQuantityDTO.Unit);
            return Ok(result);
        }

        /// <summary>Subtract two quantities</summary>
        [HttpPost("subtract")]
        [SwaggerOperation(Summary = "Subtract second quantity from first")]
        [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Subtract([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /subtract called");
            var result = _service.Subtract(
                input.ThisQuantityDTO,
                input.ThatQuantityDTO,
                input.ThatQuantityDTO.Unit);
            return Ok(result);
        }

        /// <summary>Divide first quantity by second</summary>
        [HttpPost("divide")]
        [SwaggerOperation(Summary = "Divide first quantity by second")]
        [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /divide called");
            var result = _service.Divide(input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(result);
        }

        /// <summary>Get operation history by operation type (e.g. Compare, Add, Convert)</summary>
        [HttpGet("history/operation/{operationType}")]
        [SwaggerOperation(Summary = "Get history by operation type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetByOperation(string operationType)
        {
            _logger.LogInformation("GET /history/operation/{Op} called", operationType);
            var result = _repository.GetByOperationType(operationType);
            return Ok(result);
        }

        /// <summary>Get history filtered by measurement type (e.g. Length, Weight)</summary>
        [HttpGet("history/type/{measurementType}")]
        [SwaggerOperation(Summary = "Get history by measurement type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetByMeasurementType(string measurementType)
        {
            _logger.LogInformation("GET /history/type/{Type} called", measurementType);
            var result = _repository.GetByMeasurementType(measurementType);
            return Ok(result);
        }

        /// <summary>Get all errored measurements</summary>
        [HttpGet("history/errored")]
        [SwaggerOperation(Summary = "Get all measurements that resulted in errors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetErrored()
        {
            _logger.LogInformation("GET /history/errored called");
            var result = _repository.GetErroredMeasurements();
            return Ok(result);
        }

        /// <summary>Count operations by type</summary>
        [HttpGet("count/{operationType}")]
        [SwaggerOperation(Summary = "Count operations by type")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCount(string operationType)
        {
            _logger.LogInformation("GET /count/{Op} called", operationType);
            var count = _repository.GetCountByOperationType(operationType);
            return Ok(new { operationType, count });
        }

        /// <summary>Get all measurements</summary>
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all measurement records")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GET /all called");
            var result = _repository.GetAll();
            return Ok(result);
        }
    }
}