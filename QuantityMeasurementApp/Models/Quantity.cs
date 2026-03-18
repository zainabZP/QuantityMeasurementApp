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
            // Special handling for TemperatureUnit
            if (typeof(U) == typeof(TemperatureUnit))
            {
                var tempUnit = (TemperatureUnit)(object)Unit;
                var tempTargetUnit = (TemperatureUnit)(object)targetUnit;
                double baseValue = tempUnit.ConvertToBaseUnit(Value);
                double convertedValue = tempTargetUnit.ConvertFromBaseUnit(baseValue);
                return new Quantity<U>(convertedValue, targetUnit);
            }
            
            double baseVal = ToBase(Value, Unit);
            double convertedVal = FromBase(baseVal, targetUnit);
            return new Quantity<U>(convertedVal, targetUnit);
        }

        private double ToBase(double value, U unit)
        {
            if (unit is TemperatureUnit tempUnit)
            {
                return tempUnit.ConvertToBaseUnit(value);
            }

            if (unit is LengthUnit lengthUnit)
            {
                return lengthUnit switch
                {
                    LengthUnit.INCHES => value,
                    LengthUnit.FEET => value * 12,
                    LengthUnit.YARDS => value * 36,
                    LengthUnit.CENTIMETERS => value / 2.54,
                    _ => throw new ArgumentException("Invalid LengthUnit")
                };
            }

            if (unit is WeightUnit weightUnit)
            {
                return weightUnit switch
                {
                    WeightUnit.GRAM => value,
                    WeightUnit.KILOGRAM => value * 1000,
                    WeightUnit.POUND => value * 453.592,
                    _ => throw new ArgumentException("Invalid WeightUnit")
                };
            }

            if (unit is VolumeUnit volumeUnit)
            {
                return volumeUnit switch
                {
                    VolumeUnit.MILLILITRE => value,
                    VolumeUnit.LITRE => value * 1000,
                    VolumeUnit.GALLON => value * 3785.41,
                    _ => throw new ArgumentException("Invalid VolumeUnit")
                };
            }

            throw new ArgumentException("Unknown unit type");
        }

        private double FromBase(double baseValue, U targetUnit)
        {
            if (targetUnit is TemperatureUnit tempUnit)
            {
                return tempUnit.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is LengthUnit lengthUnit)
            {
                return lengthUnit switch
                {
                    LengthUnit.INCHES => baseValue,
                    LengthUnit.FEET => baseValue / 12,
                    LengthUnit.YARDS => baseValue / 36,
                    LengthUnit.CENTIMETERS => baseValue * 2.54,
                    _ => throw new ArgumentException("Invalid LengthUnit")
                };
            }

            if (targetUnit is WeightUnit weightUnit)
            {
                return weightUnit switch
                {
                    WeightUnit.GRAM => baseValue,
                    WeightUnit.KILOGRAM => baseValue / 1000,
                    WeightUnit.POUND => baseValue / 453.592,
                    _ => throw new ArgumentException("Invalid WeightUnit")
                };
            }

            if (targetUnit is VolumeUnit volumeUnit)
            {
                return volumeUnit switch
                {
                    VolumeUnit.MILLILITRE => baseValue,
                    VolumeUnit.LITRE => baseValue / 1000,
                    VolumeUnit.GALLON => baseValue / 3785.41,
                    _ => throw new ArgumentException("Invalid VolumeUnit")
                };
            }

            throw new ArgumentException("Unknown unit type");
        }

        public bool Equals(Quantity<U> other)
        {
            if (other == null) return false;
            return Math.Abs(ToBase(Value, Unit) - ToBase(other.Value, other.Unit)) < 0.0001;
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            // Check if operation is supported for this unit type
            if (typeof(U) == typeof(TemperatureUnit))
            {
                var tempUnit = (TemperatureUnit)(object)Unit;
                tempUnit.ValidateOperationSupport("Add");
            }

            double sum = ToBase(Value, Unit) + ToBase(other.Value, other.Unit);
            return new Quantity<U>(FromBase(sum, targetUnit), targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            // Check if operation is supported for this unit type
            if (typeof(U) == typeof(TemperatureUnit))
            {
                var tempUnit = (TemperatureUnit)(object)Unit;
                tempUnit.ValidateOperationSupport("Subtract");
            }

            double diff = ToBase(Value, Unit) - ToBase(other.Value, other.Unit);
            return new Quantity<U>(FromBase(diff, targetUnit), targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            // Check if operation is supported for this unit type
            if (typeof(U) == typeof(TemperatureUnit))
            {
                var tempUnit = (TemperatureUnit)(object)Unit;
                tempUnit.ValidateOperationSupport("Divide");
            }

            if (other.Value == 0) throw new DivideByZeroException();
            double div = ToBase(Value, Unit) / ToBase(other.Value, other.Unit);
            return div;
        }

        public override string ToString()
        {
            return $"Quantity({Value}, {Unit})";
        }
    }
}