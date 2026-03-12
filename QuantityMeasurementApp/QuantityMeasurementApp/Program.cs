using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main()
        {
            var lengthService = new LengthService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Quantity Measurement App =====");
                Console.WriteLine("1. Compare Two Lengths");
                Console.WriteLine("2. Convert Length from One Unit to Another");
                Console.WriteLine("3. Add Two Lengths with Target Unit");
                Console.WriteLine("4. Exit");
                Console.Write("Enter Choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareLengths(lengthService);
                        break;
                    case "2":
                        ConvertLength();
                        break;
                    case "3":
                        AddLengths(lengthService);
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // ------------------ UC5 Compare Two Lengths ------------------
        static void CompareLengths(LengthService service)
        {
            Console.WriteLine("\n--- First Length ---");
            double val1 = ReadDouble("Enter Value: ");
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("\n--- Second Length ---");
            double val2 = ReadDouble("Enter Value: ");
            LengthUnit unit2 = ReadUnit();

            var l1 = new QuantityLength(val1, unit1);
            var l2 = new QuantityLength(val2, unit2);

            bool result = service.AreEqual(l1, l2);
            Console.WriteLine(result ? "Lengths are Equal ✅" : "Lengths are NOT Equal ❌");
        }

        // ------------------ UC5 Convert Length ------------------
        static void ConvertLength()
        {
            double val = ReadDouble("Enter Value to Convert: ");
            LengthUnit fromUnit = ReadUnit("Select Source Unit: ");
            LengthUnit toUnit = ReadUnit("Select Target Unit: ");

            double converted = ConversionService.Convert(val, fromUnit, toUnit);
            Console.WriteLine($"{val} {fromUnit} = {converted} {toUnit}");
        }

        // ------------------ UC7 Add Two Lengths with Target Unit ------------------
        static void AddLengths(LengthService service)
        {
            Console.WriteLine("\n--- First Length ---");
            double val1 = ReadDouble("Enter Value: ");
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("\n--- Second Length ---");
            double val2 = ReadDouble("Enter Value: ");
            LengthUnit unit2 = ReadUnit();

            LengthUnit targetUnit = ReadUnit("Select Target Unit for Result: ");

            var l1 = new QuantityLength(val1, unit1);
            var l2 = new QuantityLength(val2, unit2);

            // Use the new overload for UC7
            var sum = service.AddLengths(l1, l2, targetUnit);
            Console.WriteLine($"Sum: {sum.Value} {sum.Unit}");
        }

        // ------------------ UC1 & UC2 Compatibility ------------------
        public static bool CheckFeetEquality(double val1, double val2)
        {
            var l1 = new QuantityLength(val1, LengthUnit.FEET);
            var l2 = new QuantityLength(val2, LengthUnit.FEET);
            return l1.Equals(l2);
        }

        public static bool CheckInchEquality(double val1, double val2)
        {
            var l1 = new QuantityLength(val1, LengthUnit.INCHES);
            var l2 = new QuantityLength(val2, LengthUnit.INCHES);
            return l1.Equals(l2);
        }

        // ------------------ Helpers ------------------
        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double result))
                    return result;
                Console.WriteLine("Invalid input. Enter a valid number.");
            }
        }

        static LengthUnit ReadUnit(string? prompt = null)
        {
            if (prompt != null)
                Console.WriteLine(prompt);

            Console.WriteLine("1. Feet\n2. Inches\n3. Yards\n4. Centimeters");

            while (true)
            {
                Console.Write("Select Unit: ");
                string? input = Console.ReadLine();
                return input switch
                {
                    "1" => LengthUnit.FEET,
                    "2" => LengthUnit.INCHES,
                    "3" => LengthUnit.YARDS,
                    "4" => LengthUnit.CENTIMETERS,
                    _ => throw new Exception("Invalid unit selected")
                };
            }
        }
    }
}