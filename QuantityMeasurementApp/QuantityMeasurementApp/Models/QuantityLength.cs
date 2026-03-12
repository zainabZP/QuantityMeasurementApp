using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // Base unit = FEET
        public double ToFeet()
        {
            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCHES => value / 12.0,          // 12 inches = 1 foot
                LengthUnit.YARDS => value * 3.0,            // 3 feet = 1 yard
                LengthUnit.CENTIMETERS => value * 0.0328084, // 1 cm = 0.0328084 feet
                _ => throw new InvalidOperationException("Unsupported unit")
            };
        }

        public double ConvertTo(LengthUnit targetUnit)
        {
            double valueInFeet = ToFeet();
            return targetUnit switch
            {
                LengthUnit.FEET => valueInFeet,
                LengthUnit.INCHES => valueInFeet * 12.0,
                LengthUnit.YARDS => valueInFeet / 3.0,
                LengthUnit.CENTIMETERS => valueInFeet / 0.0328084,
                _ => throw new InvalidOperationException("Unsupported target unit")
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not QuantityLength other) return false;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
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