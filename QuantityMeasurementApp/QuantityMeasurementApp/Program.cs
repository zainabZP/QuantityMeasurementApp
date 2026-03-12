using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("---- FEET COMPARISON ----");
                Console.Write("Enter first value in Feet: ");
                double feet1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter second value in Feet: ");
                double feet2 = Convert.ToDouble(Console.ReadLine());

                bool feetResult = CheckFeetEquality(feet1, feet2);
                Console.WriteLine("Feet Equal? " + feetResult);

                Console.WriteLine("\n---- INCH COMPARISON ----");
                Console.Write("Enter first value in Inches: ");
                double inch1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter second value in Inches: ");
                double inch2 = Convert.ToDouble(Console.ReadLine());

                bool inchResult = CheckInchEquality(inch1, inch2);
                Console.WriteLine("Inches Equal? " + inchResult);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input! Please enter numeric values only.");
            }
        }

        public static bool CheckFeetEquality(double a, double b)
        {
            Feet f1 = new Feet(a);
            Feet f2 = new Feet(b);
            return f1.Equals(f2);
        }

        public static bool CheckInchEquality(double a, double b)
        {
            Inch i1 = new Inch(a);
            Inch i2 = new Inch(b);
            return i1.Equals(i2);
        }
    }
}