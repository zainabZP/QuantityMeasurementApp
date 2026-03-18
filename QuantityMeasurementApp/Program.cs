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
                Console.WriteLine("4. Temperature");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine()!;
                if (choice == "5") break;

                switch (choice)
                {
                    case "1": HandleGeneric<LengthUnit>("Length"); break;
                    case "2": HandleGeneric<WeightUnit>("Weight"); break;
                    case "3": HandleGeneric<VolumeUnit>("Volume"); break;
                    case "4": HandleTemperature(); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void HandleGeneric<U>(string category) where U : struct, Enum
        {
            while (true)
            {
                Console.WriteLine($"\n{category} Operations:");
                Console.WriteLine("1. Add\n2. Subtract\n3. Divide\n4. Compare\n5. Convert\n6. Back");
                string choice = Console.ReadLine()!;
                if (choice == "6") break;

                try
                {
                    double v1, v2;
                    U u1, u2, target;

                    switch (choice)
                    {
                        case "1": // Add
                            v1 = GetDouble("Enter first value: "); u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            target = SelectUnit<U>("unit to convert sum to");
                            Console.WriteLine($"Result: {ConvertFromBase(u1, u2, v1, v2, target, "+")} {target}");
                            break;

                        case "2": // Subtract
                            v1 = GetDouble("Enter first value: "); u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            target = SelectUnit<U>("unit to convert difference to");
                            Console.WriteLine($"Result: {ConvertFromBase(u1, u2, v1, v2, target, "-")} {target}");
                            break;

                        case "3": // Divide
                            v1 = GetDouble("Enter numerator value: "); u1 = SelectUnit<U>("numerator unit");
                            v2 = GetDouble("Enter denominator value: "); u2 = SelectUnit<U>("denominator unit");
                            Console.WriteLine($"Result: {ConvertToBase(u1, v1)/ConvertToBase(u2, v2)}"); break;

                        case "4": // Compare
                            v1 = GetDouble("Enter first value: "); u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            Console.WriteLine(ConvertToBase(u1,v1)==ConvertToBase(u2,v2)?"Equal":"Not Equal"); break;

                        case "5": // Convert
                            v1 = GetDouble("Enter value: "); u1 = SelectUnit<U>("current unit");
                            target = SelectUnit<U>("target unit");
                            Console.WriteLine($"Converted: {ConvertFromBase(u1, v1, target)} {target}"); break;

                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
        }

        static void HandleTemperature()
        {
            while (true)
            {
                Console.WriteLine("\nTemperature Operations:");
                Console.WriteLine("1. Add\n2. Subtract\n3. Compare\n4. Convert\n5. Back");
                string choice = Console.ReadLine()!;
                if (choice=="5") break;

                try
                {
                    double v1,v2; TemperatureUnit u1,u2,target;
                    switch(choice)
                    {
                        case "1": // Add
                            v1=GetDouble("Enter first value: "); u1=SelectUnit<TemperatureUnit>("first unit");
                            v2=GetDouble("Enter second value: "); u2=SelectUnit<TemperatureUnit>("second unit");
                            target=SelectUnit<TemperatureUnit>("unit to convert sum to");
                            Console.WriteLine($"Result: {TemperatureUnitExtensions.ConvertFromBaseUnit(target,
                                TemperatureUnitExtensions.ConvertToBaseUnit(u1,v1)+TemperatureUnitExtensions.ConvertToBaseUnit(u2,v2))} {target}"); break;

                        case "2": // Subtract
                            v1=GetDouble("Enter first value: "); u1=SelectUnit<TemperatureUnit>("first unit");
                            v2=GetDouble("Enter second value: "); u2=SelectUnit<TemperatureUnit>("second unit");
                            target=SelectUnit<TemperatureUnit>("unit to convert difference to");
                            Console.WriteLine($"Result: {TemperatureUnitExtensions.ConvertFromBaseUnit(target,
                                TemperatureUnitExtensions.ConvertToBaseUnit(u1,v1)-TemperatureUnitExtensions.ConvertToBaseUnit(u2,v2))} {target}"); break;

                        case "3": // Compare
                            v1=GetDouble("Enter first value: "); u1=SelectUnit<TemperatureUnit>("first unit");
                            v2=GetDouble("Enter second value: "); u2=SelectUnit<TemperatureUnit>("second unit");
                            Console.WriteLine(TemperatureUnitExtensions.ConvertToBaseUnit(u1,v1)==TemperatureUnitExtensions.ConvertToBaseUnit(u2,v2)?"Equal":"Not Equal"); break;

                        case "4": // Convert
                            v1=GetDouble("Enter value: "); u1=SelectUnit<TemperatureUnit>("current unit");
                            target=SelectUnit<TemperatureUnit>("target unit");
                            Console.WriteLine($"Converted: {TemperatureUnitExtensions.ConvertFromBaseUnit(target,TemperatureUnitExtensions.ConvertToBaseUnit(u1,v1))} {target}"); break;

                        default: Console.WriteLine("Invalid choice."); break;
                    }
                } catch(Exception ex){ Console.WriteLine($"Error: {ex.Message}"); }
            }
        }

        static double ConvertToBase<U>(U unit, double value)
        {
            return typeof(U)==typeof(LengthUnit)? LengthUnitExtensions.ConvertToBaseUnit((LengthUnit)(object)unit,value) :
                   typeof(U)==typeof(WeightUnit)? WeightUnitExtensions.ConvertToBaseUnit((WeightUnit)(object)unit,value) :
                   VolumeUnitExtensions.ConvertToBaseUnit((VolumeUnit)(object)unit,value);
        }

        static double ConvertFromBase<U>(U unit1, double value, U target)
        {
            return typeof(U)==typeof(LengthUnit)? LengthUnitExtensions.ConvertFromBaseUnit((LengthUnit)(object)target, ConvertToBase((LengthUnit)(object)unit1,value)) :
                   typeof(U)==typeof(WeightUnit)? WeightUnitExtensions.ConvertFromBaseUnit((WeightUnit)(object)target, ConvertToBase((WeightUnit)(object)unit1,value)) :
                   VolumeUnitExtensions.ConvertFromBaseUnit((VolumeUnit)(object)target, ConvertToBase((VolumeUnit)(object)unit1,value));
        }

        static double ConvertFromBase<U>(U unit1, U unit2, double v1,double v2,U target,string op)
        {
            double result = op=="+" ? ConvertToBase(unit1,v1)+ConvertToBase(unit2,v2) : ConvertToBase(unit1,v1)-ConvertToBase(unit2,v2);
            return typeof(U)==typeof(LengthUnit)? LengthUnitExtensions.ConvertFromBaseUnit((LengthUnit)(object)target,result) :
                   typeof(U)==typeof(WeightUnit)? WeightUnitExtensions.ConvertFromBaseUnit((WeightUnit)(object)target,result) :
                   VolumeUnitExtensions.ConvertFromBaseUnit((VolumeUnit)(object)target,result);
        }

        static double GetDouble(string prompt) { Console.Write(prompt); return double.Parse(Console.ReadLine()!); }

        static T SelectUnit<T>(string prompt) where T:struct,Enum
        {
            Console.WriteLine($"Select {prompt}:");
            var values=Enum.GetValues(typeof(T)); // typeof returns the Type object for the specified type, and Enum.GetValues returns an array of the values of the constants in the specified enumeration.
            int i=1; foreach(var val in values){Console.WriteLine($"{i}. {val}"); i++;}
            int choice=int.Parse(Console.ReadLine()!);
            if(choice<1||choice>values.Length) throw new Exception("Invalid unit choice.");
            return (T)values.GetValue(choice-1)!;
        }

        public static bool CheckFeetEquality(double feet1, double feet2)
        {
            var q1 = new Quantity<LengthUnit>(feet1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(feet2, LengthUnit.FEET);
            return q1.Equals(q2);
        }

        public static bool CheckInchEquality(double inch1, double inch2)
        {
            var q1 = new Quantity<LengthUnit>(inch1, LengthUnit.INCHES);
            var q2 = new Quantity<LengthUnit>(inch2, LengthUnit.INCHES);
            return q1.Equals(q2);
        }
    }
}