namespace QM.Models.DTOs
{
    public class QuantityDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
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