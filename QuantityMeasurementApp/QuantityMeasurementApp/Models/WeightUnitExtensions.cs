namespace QuantityMeasurementApp.Models
{
    public static class WeightUnitExtensions
    {
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.GRAM => value,
                WeightUnit.KILOGRAM => value * 1000,
                WeightUnit.POUND => value * 453.592,
                _ => value
            };
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.GRAM => baseValue,
                WeightUnit.KILOGRAM => baseValue / 1000,
                WeightUnit.POUND => baseValue / 453.592,
                _ => baseValue
            };
        }
    }
}