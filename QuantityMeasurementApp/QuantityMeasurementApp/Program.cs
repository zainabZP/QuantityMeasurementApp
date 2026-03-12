using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static LengthService service = new LengthService();

        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== UC4 Length Comparison =====");
                Console.WriteLine("1. Compare Two Lengths");
                Console.WriteLine("2. Exit");
                Console.Write("Enter Choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CompareLengths();
                        break;

                    case 2:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        static void CompareLengths()
        {
            Console.WriteLine("\n--- First Length ---");
            double v1 = ReadValue();
            LengthUnit u1 = ReadUnit();

            Console.WriteLine("\n--- Second Length ---");
            double v2 = ReadValue();
            LengthUnit u2 = ReadUnit();

            QuantityLength l1 = new QuantityLength(v1, u1);
            QuantityLength l2 = new QuantityLength(v2, u2);

            bool result = service.AreEqual(l1, l2);

            Console.WriteLine(result
                ? "Lengths are Equal"
                : "Lengths are NOT Equal");
        }

        static double ReadValue()
        {
            Console.Write("Enter Value: ");
            return Convert.ToDouble(Console.ReadLine());
        }

        static LengthUnit ReadUnit()
        {
            Console.WriteLine("1. Feet");
            Console.WriteLine("2. Inches");
            Console.WriteLine("3. Yards");
            Console.WriteLine("4. Centimeters");
            Console.Write("Select Unit: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new Exception("Invalid unit")
            };
        }

        // Required for UC2 tests
        public static bool CheckFeetEquality(double a, double b)
        {
            LengthService service = new LengthService();

            QuantityLength l1 = new QuantityLength(a, LengthUnit.FEET);
            QuantityLength l2 = new QuantityLength(b, LengthUnit.FEET);

            return service.AreEqual(l1, l2);
        }

        public static bool CheckInchEquality(double a, double b)
        {
            LengthService service = new LengthService();

            QuantityLength l1 = new QuantityLength(a, LengthUnit.INCHES);
            QuantityLength l2 = new QuantityLength(b, LengthUnit.INCHES);

            return service.AreEqual(l1, l2);
        }
    }
}