using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        public QuantityLength(double value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityLength(converted, targetUnit);
        }

        // ✅ Added for UC6 tests
        public QuantityLength Add(QuantityLength other)
        {
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;

            double result = Unit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(result, Unit);
        }

        public override bool Equals(object obj)
        {
            if (obj is not QuantityLength other)
                return false;

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Unit);
        }
    }
}