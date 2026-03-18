using System;

namespace QuantityMeasurementApp.Models
{
    public enum ArithmeticOperation
    {
        ADD,
        SUBTRACT,
        DIVIDE
    }

    public static class ArithmeticOperationExtensions
    {
        public static double Apply(this ArithmeticOperation op, double a, double b)
        {
            return op switch
            {
                ArithmeticOperation.ADD => a + b,
                ArithmeticOperation.SUBTRACT => a - b,
                ArithmeticOperation.DIVIDE => a / b,
                _ => throw new InvalidOperationException("Invalid arithmetic operation")
            };
        }
    }
}