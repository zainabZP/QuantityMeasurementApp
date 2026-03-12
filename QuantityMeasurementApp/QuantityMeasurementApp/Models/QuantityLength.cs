using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Invalid value");

            Value = value;
            Unit = unit;
        }

        public double ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = Unit.GetConversionFactor() * Value;
            double result = baseValue / targetUnit.GetConversionFactor();
            return Math.Round(result, 3);
        }

        public QuantityLength Add(QuantityLength other)
        {
            double baseA = Unit.GetConversionFactor() * Value;
            double baseB = other.Unit.GetConversionFactor() * other.Value;

            double sumBase = baseA + baseB;

            double result = sumBase / Unit.GetConversionFactor();

            return new QuantityLength(Math.Round(result, 3), Unit);
        }

        public override bool Equals(object obj)
        {
            if (obj is not QuantityLength other)
                return false;

            double baseA = Unit.GetConversionFactor() * Value;
            double baseB = other.Unit.GetConversionFactor() * other.Value;

            return Math.Abs(baseA - baseB) < 0.001;
        }

        public override int GetHashCode()
        {
            return (Unit.GetConversionFactor() * Value).GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({Value}, {Unit})";
        }
    }
}