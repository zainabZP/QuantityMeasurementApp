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
                WeightUnit.POUND => 453.592,
                _ => 1
            };
        }
    }
}