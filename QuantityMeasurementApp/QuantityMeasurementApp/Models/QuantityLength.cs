using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite.");
            Value = value;
            Unit = unit;
        }

        // Convert to base unit FEET
        public double ToFeet()
        {
            return Unit switch
            {
                LengthUnit.FEET => Value,
                LengthUnit.INCHES => Value / 12.0,
                LengthUnit.YARDS => Value * 3.0,
                LengthUnit.CENTIMETERS => Value * 0.0328084,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        // Convert to target unit
        public double ConvertTo(LengthUnit targetUnit)
        {
            double valueInFeet = ToFeet();
            return targetUnit switch
            {
                LengthUnit.FEET => valueInFeet,
                LengthUnit.INCHES => valueInFeet * 12.0,
                LengthUnit.YARDS => valueInFeet / 3.0,
                LengthUnit.CENTIMETERS => valueInFeet / 0.0328084,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        // Add two lengths
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            double sumInFeet = this.ToFeet() + other.ToFeet();
            double sumInThisUnit = ConvertFeetToThisUnit(sumInFeet);

            return new QuantityLength(Math.Round(sumInThisUnit, 6), this.Unit);
        }

        private double ConvertFeetToThisUnit(double valueInFeet)
        {
            return Unit switch
            {
                LengthUnit.FEET => valueInFeet,
                LengthUnit.INCHES => valueInFeet * 12.0,
                LengthUnit.YARDS => valueInFeet / 3.0,
                LengthUnit.CENTIMETERS => valueInFeet / 0.0328084,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not QuantityLength other) return false;
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