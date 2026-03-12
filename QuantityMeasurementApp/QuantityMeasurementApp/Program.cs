using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("\n===== UC3 : Length Equality (Feet & Inches) =====");
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
                        continueProgram = false;
                        break;

                    default:
                        Console.WriteLine("Invalid Choice!");
                        break;
                }
            }
        }

        static void CompareLengths()
        {
            Console.WriteLine("\n--- Enter First Length ---");
            double value1 = ReadValue();
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("\n--- Enter Second Length ---");
            double value2 = ReadValue();
            LengthUnit unit2 = ReadUnit();

            QuantityLength length1 = new QuantityLength(value1, unit1);
            QuantityLength length2 = new QuantityLength(value2, unit2);

            bool result = length1.Equals(length2);

            Console.WriteLine("\nResult: " + (result ? "Lengths are Equal ✅" : "Lengths are NOT Equal ❌"));
        }

        static double ReadValue()
        {
            Console.Write("Enter Value: ");
            return Convert.ToDouble(Console.ReadLine());
        }

        static LengthUnit ReadUnit()
        {
            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. Feet");
            Console.WriteLine("2. Inches");
            Console.Write("Enter choice: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                _ => throw new Exception("Invalid Unit Selected")
            };
        }

        // ===== Methods required for UC2 tests =====

        public static bool CheckFeetEquality(double a, double b)
        {
            var f1 = new QuantityLength(a, LengthUnit.FEET);
            var f2 = new QuantityLength(b, LengthUnit.FEET);
            return f1.Equals(f2);
        }

        public static bool CheckInchEquality(double a, double b)
        {
            var i1 = new QuantityLength(a, LengthUnit.INCHES);
            var i2 = new QuantityLength(b, LengthUnit.INCHES);
            return i1.Equals(i2);
        }
    }
}