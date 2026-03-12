using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Quantity Measurement App =====");
                Console.WriteLine("1. Compare Two Lengths");
                Console.WriteLine("2. Convert Length from One Unit to Another");
                Console.WriteLine("3. Exit");
                Console.Write("Enter Choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareLengths();
                        break;
                    case "2":
                        ConvertLength();
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // ======================
        // Old UC2 Stub Methods
        // ======================
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

        // ======================
        // New UC4/UC5 Methods
        // ======================
        static void CompareLengths()
        {
            Console.WriteLine("\n--- First Length ---");
            double val1 = ReadDouble("Enter Value: ");
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("\n--- Second Length ---");
            double val2 = ReadDouble("Enter Value: ");
            LengthUnit unit2 = ReadUnit();

            var l1 = new QuantityLength(val1, unit1);
            var l2 = new QuantityLength(val2, unit2);

            var service = new LengthService();
            bool result = service.AreEqual(l1, l2);

            Console.WriteLine(result ? "Lengths are Equal ✅" : "Lengths are NOT Equal ❌");
        }

        static void ConvertLength()
        {
            double val = ReadDouble("Enter Value to Convert: ");
            LengthUnit fromUnit = ReadUnit("Select Source Unit: ");
            LengthUnit toUnit = ReadUnit("Select Target Unit: ");

            double converted = ConversionService.Convert(val, fromUnit, toUnit);
            Console.WriteLine($"{val} {fromUnit} = {converted} {toUnit}");
        }

        static double ReadDouble(string prompt)
        {
            double result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (double.TryParse(input, out result))
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