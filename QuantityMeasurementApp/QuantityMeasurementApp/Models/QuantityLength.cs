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

        public double ToFeet()
        {
            return Unit.ConvertToBaseUnit(Value);
        }

        public double ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ToFeet();
            return targetUnit.ConvertFromBaseUnit(baseValue);
        }

        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double sumBase = this.ToFeet() + other.ToFeet();
            double result = this.Unit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(Math.Round(result, 6), this.Unit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not QuantityLength other)
                return false;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 1e-6;
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}