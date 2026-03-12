using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class WeightService
    {
        private double ToBaseUnit(QuantityWeight q)
        {
            switch (q.Unit)
            {
                case WeightUnit.GRAM:
                    return q.Value;

                case WeightUnit.KILOGRAM:
                    return q.Value * 1000;

                case WeightUnit.POUND:
                    return q.Value * 453.592;

                default:
                    throw new Exception("Invalid weight unit");
            }
        }

        public bool AreEqual(QuantityWeight a, QuantityWeight b)
        {
            return Math.Abs(ToBaseUnit(a) - ToBaseUnit(b)) < 0.0001;
        }
    }
}