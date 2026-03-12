using QuantityMeasurementApp.Models;
using System;

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

        // UC6: Add two lengths
        public QuantityLength AddLengths(QuantityLength l1, QuantityLength l2)
        {
            double sumInFeet = l1.ToFeet() + l2.ToFeet();

            // Convert sum back to unit of first length
            double sumInOriginalUnit = l1.Unit switch
            {
                LengthUnit.FEET => sumInFeet,
                LengthUnit.INCHES => sumInFeet * 12.0,
                LengthUnit.YARDS => sumInFeet / 3.0,
                LengthUnit.CENTIMETERS => sumInFeet / 0.0328084,
                _ => throw new InvalidOperationException("Unsupported unit")
            };

            return new QuantityLength(Math.Round(sumInOriginalUnit, 6), l1.Unit);
        }
    }
}