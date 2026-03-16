using System;

namespace QuantityMeasurementApp.Models
{
    public static class WeightUnitExtensions
    {
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.GRAM => 1,
                WeightUnit.KILOGRAM => 1000,
                WeightUnit.TONNE => 1000000,
                WeightUnit.POUND => 453.592,
                _ => throw new InvalidOperationException("Invalid WeightUnit")
            };
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}