using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class LengthService
    {
        public bool AreEqual(QuantityLength l1, QuantityLength l2)
        {
            double a = l1.ToFeet();
            double b = l2.ToFeet();

            return Math.Abs(a - b) < 0.0001;
        }
    }
}