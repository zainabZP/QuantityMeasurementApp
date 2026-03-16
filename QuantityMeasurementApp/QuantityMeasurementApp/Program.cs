using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nSelect category:");
                Console.WriteLine("1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Volume");
                Console.WriteLine("4. Exit");

                string categoryChoice = Console.ReadLine();

                if (categoryChoice == "4")
                    break;

                switch (categoryChoice)
                {
                    case "1":
                        HandleOperations<LengthUnit>("Length");
                        break;
                    case "2":
                        HandleOperations<WeightUnit>("Weight");
                        break;
                    case "3":
                        HandleOperations<VolumeUnit>("Volume");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // UC1 compatibility
        public static bool CheckFeetEquality(double a, double b)
        {
            return new Feet(a).Equals(new Feet(b));
        }

        // UC2 compatibility
        public static bool CheckInchEquality(double a, double b)
        {
            return new Inch(a).Equals(new Inch(b));
        }

        static void HandleOperations<U>(string category) where U : struct, Enum
        {
            while (true)
            {
                Console.WriteLine($"\n{category} Operations:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Subtract");
                Console.WriteLine("3. Divide");
                Console.WriteLine("4. Compare");
                Console.WriteLine("5. Convert");
                Console.WriteLine("6. Back");

                string choice = Console.ReadLine();

                if (choice == "6")
                    break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var sum = ReadQuantity<U>("first")
                                .Add(ReadQuantity<U>("second"), SelectUnit<U>("target unit"));

                            Console.WriteLine($"Result: {sum.Value} {sum.Unit}");
                            break;

                        case "2":
                            var diff = ReadQuantity<U>("first")
                                .Subtract(ReadQuantity<U>("second"), SelectUnit<U>("target unit"));

                            Console.WriteLine($"Result: {diff.Value} {diff.Unit}");
                            break;

                        case "3":
                            var quotient = ReadQuantity<U>("first")
                                .Divide(ReadQuantity<U>("second"));

                            Console.WriteLine($"Result: {quotient}");
                            break;

                        case "4":
                            var q1 = ReadQuantity<U>("first");
                            var q2 = ReadQuantity<U>("second");

                            Console.WriteLine(q1.Equals(q2)
                                ? "Quantities are equal"
                                : "Quantities are NOT equal");
                            break;

                        case "5":
                            var qty = ReadQuantity<U>("quantity");
                            var target = SelectUnit<U>("target unit");

                            var converted = qty.ConvertTo(target);

                            Console.WriteLine($"Converted: {converted.Value} {converted.Unit}");
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static Quantity<U> ReadQuantity<U>(string label) where U : struct, Enum
        {
            Console.WriteLine($"Enter {label} value:");
            double value = Convert.ToDouble(Console.ReadLine());

            U unit = SelectUnit<U>($"{label} unit");

            return new Quantity<U>(value, unit);
        }

        static U SelectUnit<U>(string prompt) where U : struct, Enum
        {
            Console.WriteLine($"Select {prompt}:");

            var values = Enum.GetValues(typeof(U));
            int i = 1;

            foreach (var val in values)
            {
                Console.WriteLine($"{i}. {val}");
                i++;
            }

            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice < 1 || choice > values.Length)
                throw new Exception("Invalid unit choice.");

            return (U)values.GetValue(choice - 1);
        }
    }
}