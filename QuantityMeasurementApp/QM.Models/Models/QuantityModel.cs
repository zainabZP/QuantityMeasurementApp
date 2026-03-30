namespace QM.Models.Models
{
    public class QuantityModel<T> where T : struct, Enum
    {
        public double Value { get; }
        public T Unit { get; }

        public QuantityModel(double value, T unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}