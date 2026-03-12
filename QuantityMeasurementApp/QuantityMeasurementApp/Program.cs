using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static QuantityService service = new QuantityService();

        static void Main(string[] args)
        {
            int choice;

            do
            {
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. Length Operations");
                Console.WriteLine("2. Weight Operations");
                Console.WriteLine("0. Exit");
                Console.Write("Enter choice: ");

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        LengthMenu();
                        break;

                    case 2:
                        WeightMenu();
                        break;

                    case 0:
                        Console.WriteLine("Exiting program...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

            } while (choice != 0);
        }

        static void LengthMenu()
        {
            Console.WriteLine("\n--- Length Operation ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add");
            Console.Write("Enter option: ");

            int option = Convert.ToInt32(Console.ReadLine());

            if (option == 1)
            {
                Console.Write("Enter first value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                LengthUnit u1 = SelectLengthUnit();

                Console.Write("Enter second value: ");
                double v2 = Convert.ToDouble(Console.ReadLine());
                LengthUnit u2 = SelectLengthUnit();

                var q1 = new QuantityLength(v1, u1);
                var q2 = new QuantityLength(v2, u2);

                Console.WriteLine(LengthService.AreEqual(q1, q2)
                    ? "Lengths are equal"
                    : "Lengths are NOT equal");
            }

            else if (option == 2)
            {
                Console.Write("Enter value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                LengthUnit u1 = SelectLengthUnit();

                var q1 = new QuantityLength(v1, u1);

                Console.WriteLine("Convert To:");
                LengthUnit target = SelectLengthUnit();

                var result = LengthService.Convert(q1, target);

                Console.WriteLine("Result: " + result.Value + " " + result.Unit);
            }

            else if (option == 3)
            {
                Console.Write("Enter first value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                LengthUnit u1 = SelectLengthUnit();

                Console.Write("Enter second value: ");
                double v2 = Convert.ToDouble(Console.ReadLine());
                LengthUnit u2 = SelectLengthUnit();

                var q1 = new QuantityLength(v1, u1);
                var q2 = new QuantityLength(v2, u2);

                Console.WriteLine("Result Unit:");
                LengthUnit target = SelectLengthUnit();

                var result = LengthService.Add(q1, q2, target);

                Console.WriteLine("Result: " + result.Value + " " + result.Unit);
            }
        }

        static void WeightMenu()
        {
            Console.WriteLine("\n--- Weight Operation ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add");
            Console.Write("Enter option: ");

            int option = Convert.ToInt32(Console.ReadLine());

            if (option == 1)
            {
                Console.Write("Enter first value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                WeightUnit u1 = SelectWeightUnit();

                Console.Write("Enter second value: ");
                double v2 = Convert.ToDouble(Console.ReadLine());
                WeightUnit u2 = SelectWeightUnit();

                var q1 = new QuantityWeight(v1, u1);
                var q2 = new QuantityWeight(v2, u2);

                WeightService weightService = new WeightService();

                Console.WriteLine(weightService.AreEqual(q1, q2)
                    ? "Weights are equal"
                    : "Weights are NOT equal");
            }

            else if (option == 2)
            {
                Console.Write("Enter value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                WeightUnit u1 = SelectWeightUnit();

                var q1 = new QuantityWeight(v1, u1);

                Console.WriteLine("Convert To:");
                WeightUnit target = SelectWeightUnit();

                var result = service.Convert(new Quantity<WeightUnit>(q1.Value, q1.Unit), target);

                Console.WriteLine("Result: " + result.Value + " " + result.Unit);
            }

            else if (option == 3)
            {
                Console.Write("Enter first value: ");
                double v1 = Convert.ToDouble(Console.ReadLine());
                WeightUnit u1 = SelectWeightUnit();

                Console.Write("Enter second value: ");
                double v2 = Convert.ToDouble(Console.ReadLine());
                WeightUnit u2 = SelectWeightUnit();

                var q1 = new QuantityWeight(v1, u1);
                var q2 = new QuantityWeight(v2, u2);

                Console.WriteLine("Result Unit:");
                WeightUnit target = SelectWeightUnit();

                var result = service.Add(
                    new Quantity<WeightUnit>(q1.Value, q1.Unit),
                    new Quantity<WeightUnit>(q2.Value, q2.Unit),
                    target);

                Console.WriteLine("Result: " + result.Value + " " + result.Unit);
            }
        }

        static LengthUnit SelectLengthUnit()
        {
            Console.WriteLine("Select Length Unit:");
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARD");
            Console.WriteLine("4. CENTIMETERS");

            int choice = Convert.ToInt32(Console.ReadLine());
            return (LengthUnit)(choice - 1);
        }

        static WeightUnit SelectWeightUnit()
        {
            Console.WriteLine("Select Weight Unit:");
            Console.WriteLine("1. GRAM");
            Console.WriteLine("2. KILOGRAM");
            Console.WriteLine("3. POUND");

            int choice = Convert.ToInt32(Console.ReadLine());
            return (WeightUnit)(choice - 1);
        }

        // Legacy helpers for UC2 tests
        public static bool CheckFeetEquality(double val1, double val2)
        {
            var a = new QuantityLength(val1, LengthUnit.FEET);
            var b = new QuantityLength(val2, LengthUnit.FEET);
            return LengthService.AreEqual(a, b);
        }

        public static bool CheckInchEquality(double val1, double val2)
        {
            var a = new QuantityLength(val1, LengthUnit.INCHES);
            var b = new QuantityLength(val2, LengthUnit.INCHES);
            return LengthService.AreEqual(a, b);
        }
    }
}