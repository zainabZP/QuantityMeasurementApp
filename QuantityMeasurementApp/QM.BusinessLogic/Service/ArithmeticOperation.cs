using System;
using QM.Models.Models;

namespace QM.BusinessLogic.Service
{
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
