using System;

namespace QM.Models.Models
{
    public static class VolumeUnitExtensions
    {
        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.MILLILITRE => 1.0,
                VolumeUnit.LITRE => 1000.0,
                VolumeUnit.GALLON => 3785.41,
                _ => throw new InvalidOperationException("Invalid VolumeUnit")
            };
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}