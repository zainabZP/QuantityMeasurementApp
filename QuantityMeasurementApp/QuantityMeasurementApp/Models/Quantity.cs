using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : struct, Enum
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBase(Value, Unit);
            double converted = FromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            double result = base1 + base2;

            return new Quantity<U>(FromBase(result, targetUnit), targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            double result = base1 - base2;

            return new Quantity<U>(FromBase(result, targetUnit), targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            return base1 / base2;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        // ✅ ADD THIS METHOD
        public override string ToString()
        {
            return $"Quantity({Value}, {Unit})";
        }

        private static double ToBase(double value, U unit)
        {
            object u = unit;

            if (u is LengthUnit lu)
                return lu.ConvertToBaseUnit(value);

            if (u is WeightUnit wu)
                return wu.ConvertToBaseUnit(value);

            if (u is VolumeUnit vu)
                return vu.ConvertToBaseUnit(value);

            throw new Exception("Unsupported unit");
        }

        private static double FromBase(double value, U unit)
        {
            object u = unit;

            if (u is LengthUnit lu)
                return lu.ConvertFromBaseUnit(value);

            if (u is WeightUnit wu)
                return wu.ConvertFromBaseUnit(value);

            if (u is VolumeUnit vu)
                return vu.ConvertFromBaseUnit(value);

            throw new Exception("Unsupported unit");
        }
    }
}