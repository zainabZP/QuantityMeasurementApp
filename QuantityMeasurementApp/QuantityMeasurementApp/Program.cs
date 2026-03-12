using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            LengthService service = new LengthService();
            WeightService weightService = new WeightService();

            while (true)
            {
                Console.WriteLine("\n===== Quantity Measurement App =====");
                Console.WriteLine("1. Compare Lengths");
                Console.WriteLine("2. Add Lengths");
                Console.WriteLine("3. Convert Length");
                Console.WriteLine("4. Compare Weights");
                Console.WriteLine("5. Exit");

                Console.Write("Enter choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                    {
                        QuantityLength l1 = ReadLength();
                        QuantityLength l2 = ReadLength();

                        bool result = LengthService.AreEqual(l1, l2);

                        Console.WriteLine(result
                            ? "Lengths are equal"
                            : "Lengths are NOT equal");
                        break;
                    }

                    case 2:
                    {
                        Console.WriteLine("1. Add without target unit");
                        Console.WriteLine("2. Add with target unit");

                        int addChoice = int.Parse(Console.ReadLine());

                        QuantityLength l1 = ReadLength();
                        QuantityLength l2 = ReadLength();

                        if (addChoice == 1)
                        {
                            QuantityLength result = service.AddLengths(l1, l2);
                            Console.WriteLine("Result: " + result);
                        }
                        else
                        {
                            Console.WriteLine("Select target unit:");
                            LengthUnit target = ReadLengthUnit();

                            QuantityLength result =
                                service.AddLengths(l1, l2, target);

                            Console.WriteLine("Result: " + result);
                        }

                        break;
                    }

                    case 3:
                    {
                        QuantityLength q = ReadLength();

                        Console.WriteLine("Convert to:");
                        LengthUnit target = ReadLengthUnit();

                        QuantityLength result =
                            LengthService.Convert(q, target);

                        Console.WriteLine("Converted: " + result);
                        break;
                    }

                    case 4:
                    {
                        QuantityWeight w1 = ReadWeight();
                        QuantityWeight w2 = ReadWeight();

                        bool result = weightService.AreEqual(w1, w2);

                        Console.WriteLine(result
                            ? "Weights are equal"
                            : "Weights are NOT equal");

                        break;
                    }

                    case 5:
                        return;
                }
            }
        }

        // Required by UC2 Tests
        public static bool CheckFeetEquality(double a, double b)
        {
            Feet f1 = new Feet(a);
            Feet f2 = new Feet(b);
            return f1.Equals(f2);
        }

        // Required by UC2 Tests
        public static bool CheckInchEquality(double a, double b)
        {
            Inch i1 = new Inch(a);
            Inch i2 = new Inch(b);
            return i1.Equals(i2);
        }

        static QuantityLength ReadLength()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine());

            LengthUnit unit = ReadLengthUnit();

            return new QuantityLength(value, unit);
        }

        static LengthUnit ReadLengthUnit()
        {
            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. Feet");
            Console.WriteLine("2. Inches");
            Console.WriteLine("3. Yards");
            Console.WriteLine("4. Centimeters");

            int unit = int.Parse(Console.ReadLine());

            return unit switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid Unit")
            };
        }

        static QuantityWeight ReadWeight()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine());

            Console.WriteLine("Select Weight Unit:");
            Console.WriteLine("1. Gram");
            Console.WriteLine("2. Kilogram");
            Console.WriteLine("3. Pound");

            int unit = int.Parse(Console.ReadLine());

            WeightUnit weightUnit = unit switch
            {
                1 => WeightUnit.GRAM,
                2 => WeightUnit.KILOGRAM,
                3 => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };

            return new QuantityWeight(value, weightUnit);
        }
    }
}