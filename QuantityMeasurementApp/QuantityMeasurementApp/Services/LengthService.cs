using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class LengthService
    {
        public static bool AreEqual(QuantityLength a, QuantityLength b)
        {
            double baseA = a.Unit.GetConversionFactor() * a.Value;
            double baseB = b.Unit.GetConversionFactor() * b.Value;

            return Math.Abs(baseA - baseB) < 0.001;
        }

        public static QuantityLength Convert(QuantityLength q, LengthUnit targetUnit)
        {
            double baseValue = q.Unit.GetConversionFactor() * q.Value;
            double result = baseValue / targetUnit.GetConversionFactor();

            return new QuantityLength(Math.Round(result, 3), targetUnit);
        }

        public QuantityLength AddLengths(QuantityLength a, QuantityLength b)
        {
            return Add(a, b);
        }

        public QuantityLength AddLengths(
            QuantityLength a,
            QuantityLength b,
            LengthUnit targetUnit)
        {
            return Add(a, b, targetUnit);
        }

        public static QuantityLength Add(QuantityLength a, QuantityLength b)
        {
            double baseA = a.Unit.GetConversionFactor() * a.Value;
            double baseB = b.Unit.GetConversionFactor() * b.Value;

            double sumBase = baseA + baseB;

            double result = sumBase / a.Unit.GetConversionFactor();

            return new QuantityLength(Math.Round(result, 3), a.Unit);
        }

        public static QuantityLength Add(
            QuantityLength a,
            QuantityLength b,
            LengthUnit targetUnit)
        {
            double baseA = a.Unit.GetConversionFactor() * a.Value;
            double baseB = b.Unit.GetConversionFactor() * b.Value;

            double sumBase = baseA + baseB;

            double result = sumBase / targetUnit.GetConversionFactor();

            return new QuantityLength(Math.Round(result, 3), targetUnit);
        }
    }
}