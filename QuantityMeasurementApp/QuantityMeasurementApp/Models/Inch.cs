using System;

namespace QuantityMeasurementApp.Models
{
    public class Inch
    {
        private readonly double value;

        public Inch(double value)
        {
            this.value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || obj.GetType() != typeof(Inch))
                return false;

            Inch other = (Inch)obj;

            return Math.Abs(this.value - other.value) < 0.0001;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}