using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : Enum
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }

        // Convert to another unit
        public Quantity<U> ConvertTo(U toUnit)
        {
            double baseValue = ConvertToBase(Value, Unit);
            double result = ConvertFromBase(baseValue, toUnit);
            return new Quantity<U>(result, toUnit);
        }

        // Add two quantities
        public Quantity<U> Add(Quantity<U> other, U resultUnit)
        {
            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            double sumBase = base1 + base2;
            double result = ConvertFromBase(sumBase, resultUnit);

            return new Quantity<U>(result, resultUnit);
        }

        // Compare quantities
        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ConvertToBase(Value, Unit);
            double base2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }

        // ---------- Helper Conversion ----------

        private static double ConvertToBase(double value, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (unit is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            if (unit is VolumeUnit vu)
                return vu.ConvertToBaseUnit(value);

            throw new InvalidOperationException("Unsupported unit type");
        }

        private static double ConvertFromBase(double baseValue, U unit)
        {
            if (unit is LengthUnit lu)
                return lu.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit wu)
                return wu.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit vu)
                return vu.ConvertFromBaseUnit(baseValue);

            throw new InvalidOperationException("Unsupported unit type");
        }

        public override string ToString()
        {
            return $"Quantity({Value}, {Unit})";
        }
    }
}