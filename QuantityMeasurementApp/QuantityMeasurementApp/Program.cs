using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        public static void Main()
        {
            LengthService service = new LengthService();

            while (true)
            {
                Console.WriteLine("\n===== Quantity Measurement App =====");
                Console.WriteLine("1. Convert Units");
                Console.WriteLine("2. Add Lengths (Without Target Unit)");
                Console.WriteLine("3. Add Lengths (With Target Unit)");
                Console.WriteLine("4. Compare Lengths");
                Console.WriteLine("5. Exit");

                Console.Write("Enter choice: ");
                int choice = int.Parse(Console.ReadLine()!);

                switch (choice)
                {
                    case 1:
                        ConvertUnits();
                        break;

                    case 2:
                        AddLengthsWithoutTarget(service);
                        break;

                    case 3:
                        AddLengthsWithTarget();
                        break;

                    case 4:
                        CompareLengths();
                        break;

                    case 5:
                        return;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        // ===== UC1 & UC2 test support =====

        public static bool CheckFeetEquality(double a, double b)
        {
            var f1 = new Feet(a);
            var f2 = new Feet(b);
            return f1.Equals(f2);
        }

        public static bool CheckInchEquality(double a, double b)
        {
            var i1 = new Inch(a);
            var i2 = new Inch(b);
            return i1.Equals(i2);
        }

        // ===== Menu helpers =====

        static LengthUnit ReadUnit()
        {
            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");

            int option = int.Parse(Console.ReadLine()!);

            return option switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        // ===== UC8 =====
        static void ConvertUnits()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            LengthUnit from = ReadUnit();
            LengthUnit to = ReadUnit();

            var q = new QuantityLength(value, from);
            var result = LengthService.Convert(q, to);

            Console.WriteLine($"Result: {result}");
        }

        // ===== UC6 =====
        static void AddLengthsWithoutTarget(LengthService service)
        {
            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);
            LengthUnit u1 = ReadUnit();

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);
            LengthUnit u2 = ReadUnit();

            var a = new QuantityLength(v1, u1);
            var b = new QuantityLength(v2, u2);

            var result = service.AddLengths(a, b);

            Console.WriteLine($"Result: {result}");
        }

        // ===== UC7 / UC8 =====
        static void AddLengthsWithTarget()
        {
            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);
            LengthUnit u1 = ReadUnit();

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);
            LengthUnit u2 = ReadUnit();

            Console.WriteLine("Select Target Unit:");
            LengthUnit target = ReadUnit();

            var a = new QuantityLength(v1, u1);
            var b = new QuantityLength(v2, u2);

            var result = LengthService.Add(a, b, target);

            Console.WriteLine($"Result: {result}");
        }

        // ===== UC5 =====
        static void CompareLengths()
        {
            Console.Write("Enter first value: ");
            double v1 = double.Parse(Console.ReadLine()!);
            LengthUnit u1 = ReadUnit();

            Console.Write("Enter second value: ");
            double v2 = double.Parse(Console.ReadLine()!);
            LengthUnit u2 = ReadUnit();

            var a = new QuantityLength(v1, u1);
            var b = new QuantityLength(v2, u2);

            Console.WriteLine($"Equal: {LengthService.AreEqual(a, b)}");
        }
    }
}