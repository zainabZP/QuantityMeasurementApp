using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.Write("Enter first value in Feet: ");
                string input1 = Console.ReadLine();

                Console.Write("Enter second value in Feet: ");
                string input2 = Console.ReadLine();

                double value1 = Convert.ToDouble(input1);
                double value2 = Convert.ToDouble(input2);

                Feet f1 = new Feet(value1);
                Feet f2 = new Feet(value2);

                bool result = f1.Equals(f2);

                Console.WriteLine("Are both values equal? " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid input! Please enter numeric values.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}