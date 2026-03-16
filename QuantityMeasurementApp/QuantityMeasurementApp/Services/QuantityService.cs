using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityService<U> where U : Enum
    {
        public bool Compare(Quantity<U> q1, Quantity<U> q2)
        {
            return q1.Equals(q2);
        }

        public Quantity<U> Convert(Quantity<U> quantity, U toUnit)
        {
            return quantity.ConvertTo(toUnit);
        }

        public Quantity<U> Add(Quantity<U> q1, Quantity<U> q2, U resultUnit)
        {
            return q1.Add(q2, resultUnit);
        }
    }
}