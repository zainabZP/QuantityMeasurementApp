using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class WeightService
    {
        private double ToBaseUnit(QuantityWeight q)
        {
            return q.Unit switch
            {
                WeightUnit.GRAM => q.Value,
                WeightUnit.KILOGRAM => q.Value * 1000,
                WeightUnit.POUND => q.Value * 453.592,
                _ => throw new InvalidOperationException("Invalid WeightUnit")
            };
        }

        public bool AreEqual(QuantityWeight a, QuantityWeight b)
        {
            return Math.Abs(ToBaseUnit(a) - ToBaseUnit(b)) < 0.0001;
        }
    }
}