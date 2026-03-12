using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class LengthService
    {
        // UC5 equality
        public static bool AreEqual(QuantityLength a, QuantityLength b)
        {
            double baseA = a.Unit.ConvertToBaseUnit(a.Value);
            double baseB = b.Unit.ConvertToBaseUnit(b.Value);

            return Math.Abs(baseA - baseB) < 0.001;
        }

        // UC8 conversion (required by tests)
        public static QuantityLength Convert(QuantityLength q, LengthUnit targetUnit)
        {
            double baseValue = q.Unit.ConvertToBaseUnit(q.Value);
            double result = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityLength(Math.Round(result, 3), targetUnit);
        }

        // UC6 addition (instance version)
        public QuantityLength AddLengths(QuantityLength a, QuantityLength b)
        {
            return Add(a, b);
        }

        // UC7 addition with target unit (instance version)
        public QuantityLength AddLengths(
            QuantityLength a,
            QuantityLength b,
            LengthUnit targetUnit)
        {
            return Add(a, b, targetUnit);
        }

        // UC8 static Add(a,b)
        public static QuantityLength Add(QuantityLength a, QuantityLength b)
        {
            double baseA = a.Unit.ConvertToBaseUnit(a.Value);
            double baseB = b.Unit.ConvertToBaseUnit(b.Value);

            double sumBase = baseA + baseB;

            double result = a.Unit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(Math.Round(result, 3), a.Unit);
        }

        // UC8 static Add(a,b,targetUnit)
        public static QuantityLength Add(
            QuantityLength a,
            QuantityLength b,
            LengthUnit targetUnit)
        {
            double baseA = a.Unit.ConvertToBaseUnit(a.Value);
            double baseB = b.Unit.ConvertToBaseUnit(b.Value);

            double sumBase = baseA + baseB;

            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(Math.Round(result, 3), targetUnit);
        }
    }
}