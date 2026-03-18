using QM.Models.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityService<U> where U : struct, Enum, IMeasurable<U>
    {
        public Quantity<U> Convert(Quantity<U> quantity, U targetUnit)
        {
            return quantity.ConvertTo(targetUnit);
        }

        public Quantity<U> Add(Quantity<U> q1, Quantity<U> q2, U targetUnit)
        {
            return q1.Add(q2, targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> q1, Quantity<U> q2, U targetUnit)
        {
            return q1.Subtract(q2, targetUnit);
        }

        public double Divide(Quantity<U> q1, Quantity<U> q2)
        {
            return q1.Divide(q2);
        }

        public bool Compare(Quantity<U> q1, Quantity<U> q2)
        {
            return q1.Equals(q2);
        }
    }
}