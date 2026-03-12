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

        // Convert everything to FEET (base unit)
        private double ToFeet()
        {
            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCHES => value / 12.0,
                _ => throw new InvalidOperationException("Unsupported unit")
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(QuantityLength)) return false;

            var other = (QuantityLength)obj;

            double a = this.ToFeet();
            double b = other.ToFeet();

            return Math.Abs(a - b) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }
}