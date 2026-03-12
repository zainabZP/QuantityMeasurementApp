using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Services
{
    public class LengthService
    {
        // ------------------ UC5: Compare Lengths ------------------
        public bool AreEqual(QuantityLength l1, QuantityLength l2)
        {
            double a = l1.ToFeet();
            double b = l2.ToFeet();
            return Math.Abs(a - b) < 0.0001;
        }

        // ------------------ UC6: Add Lengths (default: first operand unit) ------------------
        public QuantityLength AddLengths(QuantityLength l1, QuantityLength l2)
        {
            double sumInFeet = l1.ToFeet() + l2.ToFeet();

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

        // ------------------ UC7: Add Lengths with Target Unit ------------------
        public QuantityLength AddLengths(QuantityLength l1, QuantityLength l2, LengthUnit targetUnit)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentException("Length cannot be null");

            double l1Feet = l1.ToFeet();
            double l2Feet = l2.ToFeet();

            double sumFeet = l1Feet + l2Feet;

            double result = targetUnit switch
            {
                LengthUnit.FEET => sumFeet,
                LengthUnit.INCHES => sumFeet * 12.0,
                LengthUnit.YARDS => sumFeet / 3.0,
                LengthUnit.CENTIMETERS => sumFeet / 0.0328084,
                _ => throw new InvalidOperationException("Unsupported target unit")
            };

            return new QuantityLength(Math.Round(result, 6), targetUnit);
        }
    }
}