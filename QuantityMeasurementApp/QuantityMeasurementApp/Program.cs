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
                Console.WriteLine("\n==== Quantity Measurement ====");
                Console.WriteLine("1. Length\n2. Weight\n3. Volume\n0. Exit");
                int choice = ReadInt("Enter choice: ");

                switch (choice)
                {
                    case 1: QuantityMenu<LengthUnit>(); break;
                    case 2: QuantityMenu<WeightUnit>(); break;
                    case 3: QuantityMenu<VolumeUnit>(); break;
                    case 0: return;
                    default: Console.WriteLine("Invalid choice"); break;
                }
            }
        }

        // UC1 compatibility
        public static bool CheckFeetEquality(double a, double b) => new Feet(a).Equals(new Feet(b));
        public static bool CheckInchEquality(double a, double b) => new Inch(a).Equals(new Inch(b));

        // Generic Menu
        static void QuantityMenu<T>() where T : Enum
        {
            Console.WriteLine("\n1. Compare\n2. Convert\n3. Add");
            int choice = ReadInt("Enter choice: ");

            switch (choice)
            {
                case 1: Compare<T>(); break;
                case 2: Convert<T>(); break;
                case 3: Add<T>(); break;
            }
        }

        static void Compare<T>() where T : Enum
        {
            var q1 = ReadQuantity<T>("First");
            var q2 = ReadQuantity<T>("Second");
            Console.WriteLine(q1.Equals(q2) ? "Equal" : "Not Equal");
        }

        static void Convert<T>() where T : Enum
        {
            var q = ReadQuantity<T>("Value");
            T toUnit = ReadUnit<T>("Convert To");
            var result = q.ConvertTo(toUnit);
            Console.WriteLine($"Converted: {result.Value} {result.Unit}");
        }

        static void Add<T>() where T : Enum
        {
            var q1 = ReadQuantity<T>("First");
            var q2 = ReadQuantity<T>("Second");
            T resultUnit = ReadUnit<T>("Result Unit");

            var sum = q1.Add(q2, resultUnit);
            Console.WriteLine($"Sum: {sum.Value} {sum.Unit}");
        }

        // Helpers
        static Quantity<T> ReadQuantity<T>(string label) where T : Enum
        {
            double value = ReadDouble($"Enter {label} value: ");
            T unit = ReadUnit<T>($"{label} unit");
            return new Quantity<T>(value, unit);
        }

        static T ReadUnit<T>(string label) where T : Enum
        {
            var units = Enum.GetValues(typeof(T));
            Console.WriteLine(label + ":");

            for (int i = 0; i < units.Length; i++)
                Console.WriteLine($"{i + 1}. {units.GetValue(i)}");

            int choice = ReadInt("Select: ") - 1;
            return (T)units.GetValue(choice)!;
        }

        static int ReadInt(string msg)
        {
            Console.Write(msg);
            return int.Parse(Console.ReadLine() ?? "0");
        }

        static double ReadDouble(string msg)
        {
            Console.Write(msg);
            return double.Parse(Console.ReadLine() ?? "0");
        }
    }
}