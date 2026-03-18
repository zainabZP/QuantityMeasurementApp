using QM.Models.Models;

namespace QuantityMeasurementApp.Services
{
    public class ConversionService
    {
        public double ConvertLength(double value, LengthUnit from, LengthUnit to)
        {
            double baseValue = from.ConvertToBaseUnit(value);
            return to.ConvertFromBaseUnit(baseValue);
        }

        public double ConvertWeight(double value, WeightUnit from, WeightUnit to)
        {
            double baseValue = from.ConvertToBaseUnit(value);
            return to.ConvertFromBaseUnit(baseValue);
        }
    }
}