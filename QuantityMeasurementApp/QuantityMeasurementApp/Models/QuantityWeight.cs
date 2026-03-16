namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight
    {
        public double Value { get; }
        public WeightUnit Unit { get; }

        public QuantityWeight(double value, WeightUnit unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}