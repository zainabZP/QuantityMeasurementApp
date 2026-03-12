using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class LengthService
    {
        // UC5
        public static bool AreEqual(QuantityLength a, QuantityLength b)
        {
            double baseA = a.Unit.ConvertToBaseUnit(a.Value);
            double baseB = b.Unit.ConvertToBaseUnit(b.Value);

            return Math.Abs(baseA - baseB) < 0.001;
        }

        // UC6
        public QuantityLength AddLengths(QuantityLength l1, QuantityLength l2)
        {
            return l1.Add(l2);
        }

        // UC7
        public QuantityLength AddLengths(QuantityLength l1, QuantityLength l2, LengthUnit targetUnit)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentException("Length cannot be null");

            double sumBase = l1.ToFeet() + l2.ToFeet();
            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityLength(Math.Round(result, 6), targetUnit);
        }

        // UC8
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

        // UC8
        public static QuantityLength Convert(QuantityLength q, LengthUnit targetUnit)
        {
            double baseValue = q.Unit.ConvertToBaseUnit(q.Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityLength(Math.Round(converted, 3), targetUnit);
        }
    }
}