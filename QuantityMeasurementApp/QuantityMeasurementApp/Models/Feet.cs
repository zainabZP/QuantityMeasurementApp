using System;

namespace QuantityMeasurementApp.Models
{
    // Represents Feet measurement
    public class Feet
    {
        private readonly double value;

        public Feet(double value)
        {
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null)
                return false;

            if (obj.GetType() != typeof(Feet))
                return false;

            Feet other = (Feet)obj;

            return Math.Abs(this.value - other.value) < 0.0001;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}