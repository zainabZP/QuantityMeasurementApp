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

        // UC13 Enum for centralized arithmetic
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBase(Value, Unit);
            double converted = FromBase(baseValue, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // ADD
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            validateArithmeticOperands(other);

            double resultBase = performBaseArithmetic(other, ArithmeticOperation.ADD);

            double converted = FromBase(resultBase, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // SUBTRACT
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            validateArithmeticOperands(other);

            double resultBase = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

            double converted = FromBase(resultBase, targetUnit);

            return new Quantity<U>(converted, targetUnit);
        }

        // DIVIDE
        public double Divide(Quantity<U> other)
        {
            validateArithmeticOperands(other);

            return performBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        // UC13 Central Arithmetic Logic
        private double performBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            return operation switch
            {
                ArithmeticOperation.ADD => base1 + base2,

                ArithmeticOperation.SUBTRACT => base1 - base2,

                ArithmeticOperation.DIVIDE => base2 == 0
                    ? throw new ArithmeticException("Division by zero")
                    : base1 / base2,

                _ => throw new InvalidOperationException("Invalid operation")
            };
        }

        // UC13 Central Validation
        private void validateArithmeticOperands(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Operand cannot be null");

            if (!double.IsFinite(Value) || !double.IsFinite(other.Value))
                throw new ArgumentException("Values must be finite numbers");
        }

        public override bool Equals(object obj)
        {
            if (obj is not Quantity<U> other)
                return false;

            double base1 = ToBase(Value, Unit);
            double base2 = ToBase(other.Value, other.Unit);

            return Math.Abs(base1 - base2) < 0.0001;
        }

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