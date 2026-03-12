using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U>
    {
        private readonly double value;
        private readonly U unit;

        // Compatibility properties for old tests
        public double Value => value;
        public U Unit => unit;

        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid value");

            this.value = value;
            this.unit = unit;
        }

        public double ToBaseUnit()
        {
            if (unit is LengthUnit lu)
                return value * lu.GetConversionFactor();

            if (unit is WeightUnit wu)
                return value * wu.GetConversionFactor();

            throw new Exception("Unsupported unit");
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBaseUnit();
            double result = baseValue;

            if (targetUnit is LengthUnit lu)
                result = baseValue / lu.GetConversionFactor();

            if (targetUnit is WeightUnit wu)
                result = baseValue / wu.GetConversionFactor();

            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double sumBase = this.ToBaseUnit() + other.ToBaseUnit();
            double result = sumBase;

            if (targetUnit is LengthUnit lu)
                result = sumBase / lu.GetConversionFactor();

            if (targetUnit is WeightUnit wu)
                result = sumBase / wu.GetConversionFactor();

            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Quantity<U>))
                return false;

            Quantity<U> other = (Quantity<U>)obj;

            if (this.unit.GetType() != other.unit.GetType())
                return false;

            return Math.Abs(this.ToBaseUnit() - other.ToBaseUnit()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToBaseUnit().GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}