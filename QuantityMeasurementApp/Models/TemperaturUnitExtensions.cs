using System;

namespace QuantityMeasurementApp.Models
{
    public static class TemperatureUnitExtensions
    {
        // base unit: Kelvin
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.KELVIN => value,
                TemperatureUnit.CELSIUS => value + 273.15,
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5.0 / 9.0 + 273.15,
                _ => throw new InvalidOperationException("Invalid TemperatureUnit")
            };
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.KELVIN => baseValue,
                TemperatureUnit.CELSIUS => baseValue - 273.15,
                TemperatureUnit.FAHRENHEIT => (baseValue - 273.15) * 9.0 / 5.0 + 32,
                _ => throw new InvalidOperationException("Invalid TemperatureUnit")
            };
        }

        // Temperature does NOT support arithmetic operations
        public static bool SupportsOperation(this TemperatureUnit unit)
        {
            return false;
        }

        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new UnsupportedOperationException($"Temperature ({unit}) does not support {operation} operation.");
        }
    }
}