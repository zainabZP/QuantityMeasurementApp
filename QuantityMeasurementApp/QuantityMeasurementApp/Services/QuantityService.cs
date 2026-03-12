using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityService
    {
        public bool AreEqual<U>(Quantity<U> a, Quantity<U> b)
        {
            return a.Equals(b);
        }

        public Quantity<U> Convert<U>(Quantity<U> q, U target)
        {
            return q.ConvertTo(target);
        }

        public Quantity<U> Add<U>(Quantity<U> a, Quantity<U> b, U target)
        {
            return a.Add(b, target);
        }
    }
}