using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class ConversionService
    {
        public static double Convert(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            var q = new QuantityLength(value, fromUnit);
            return q.ConvertTo(toUnit);
        }
    }
}