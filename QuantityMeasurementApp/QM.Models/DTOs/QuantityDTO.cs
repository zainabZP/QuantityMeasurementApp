using System.ComponentModel.DataAnnotations;
namespace QM.Models.DTOs
{
    public class QuantityDTO
    {
        [Range(double.MinValue, double.MaxValue)]
        public double Value { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "MeasurementType is required")]
        public string MeasurementType { get; set; }

        public QuantityDTO()
        {
            Unit = string.Empty;
            MeasurementType = string.Empty;
        }

        public QuantityDTO(double value, string unit, string measurementType)
        {
            Value = value;
            Unit = unit.ToUpper();
            MeasurementType = measurementType;
        }

        public override string ToString()
        {
            return $"{Value} {Unit} ({MeasurementType})";
        }
    }
}
