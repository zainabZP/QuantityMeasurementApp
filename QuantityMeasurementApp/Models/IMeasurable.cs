using System;

namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable<U>
    {
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);

        // Functional interface to indicate if arithmetic is supported
        public delegate bool SupportsArithmetic();

        SupportsArithmetic supportsArithmetic { get; }

        // Default method to check if arithmetic is supported
        public bool SupportsOperation() => supportsArithmetic?.Invoke() ?? true;

        // Default method to validate arithmetic operation
        public void ValidateOperationSupport(string operation)
        {
            if (!SupportsOperation())
                throw new UnsupportedOperationException($"{typeof(U).Name} does not support {operation} operation.");
        }
    }

    public class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException(string message) : base(message) { }
    }
}