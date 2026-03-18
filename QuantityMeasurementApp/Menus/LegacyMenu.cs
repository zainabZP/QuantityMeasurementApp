using QM.Models.Models;

namespace QuantityMeasurementApp.Menus
{
    public class LegacyMenu
    {
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════╗");
                Console.WriteLine("║     UC1–UC14 Legacy Menu     ║");
                Console.WriteLine("╚══════════════════════════════╝");
                Console.WriteLine("1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Volume");
                Console.WriteLine("4. Temperature");
                Console.WriteLine("5. Back");

                string choice = Console.ReadLine()!;
                if (choice == "5") break;

                switch (choice)
                {
                    case "1": HandleGeneric<LengthUnit>("Length"); break;
                    case "2": HandleGeneric<WeightUnit>("Weight"); break;
                    case "3": HandleGeneric<VolumeUnit>("Volume"); break;
                    case "4": HandleTemperature();                 break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        private void HandleGeneric<U>(string category) where U : struct, Enum
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
                        case "1":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            target = SelectUnit<U>("unit to convert sum to");
                            Console.WriteLine($"Result: {ConvertFromBase(u1, u2, v1, v2, target, "+")} {target}");
                            break;
                        case "2":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            target = SelectUnit<U>("unit to convert difference to");
                            Console.WriteLine($"Result: {ConvertFromBase(u1, u2, v1, v2, target, "-")} {target}");
                            break;
                        case "3":
                            v1 = GetDouble("Enter numerator value: ");   u1 = SelectUnit<U>("numerator unit");
                            v2 = GetDouble("Enter denominator value: "); u2 = SelectUnit<U>("denominator unit");
                            Console.WriteLine($"Result: {ConvertToBase(u1, v1) / ConvertToBase(u2, v2)}");
                            break;
                        case "4":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<U>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<U>("second unit");
                            Console.WriteLine(ConvertToBase(u1, v1) == ConvertToBase(u2, v2) ? "Equal" : "Not Equal");
                            break;
                        case "5":
                            v1 = GetDouble("Enter value: "); u1 = SelectUnit<U>("current unit");
                            target = SelectUnit<U>("target unit");
                            Console.WriteLine($"Converted: {ConvertFromBase(u1, v1, target)} {target}");
                            break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
        }

        private void HandleTemperature()
        {
            while (true)
            {
                Console.WriteLine("\nTemperature Operations:");
                Console.WriteLine("1. Add\n2. Subtract\n3. Compare\n4. Convert\n5. Back");
                string choice = Console.ReadLine()!;
                if (choice == "5") break;

                try
                {
                    double v1, v2; TemperatureUnit u1, u2, target;
                    switch (choice)
                    {
                        case "1":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<TemperatureUnit>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<TemperatureUnit>("second unit");
                            target = SelectUnit<TemperatureUnit>("unit to convert sum to");
                            Console.WriteLine($"Result: {TemperatureUnitExtensions.ConvertFromBaseUnit(target, TemperatureUnitExtensions.ConvertToBaseUnit(u1, v1) + TemperatureUnitExtensions.ConvertToBaseUnit(u2, v2))} {target}");
                            break;
                        case "2":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<TemperatureUnit>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<TemperatureUnit>("second unit");
                            target = SelectUnit<TemperatureUnit>("unit to convert difference to");
                            Console.WriteLine($"Result: {TemperatureUnitExtensions.ConvertFromBaseUnit(target, TemperatureUnitExtensions.ConvertToBaseUnit(u1, v1) - TemperatureUnitExtensions.ConvertToBaseUnit(u2, v2))} {target}");
                            break;
                        case "3":
                            v1 = GetDouble("Enter first value: ");  u1 = SelectUnit<TemperatureUnit>("first unit");
                            v2 = GetDouble("Enter second value: "); u2 = SelectUnit<TemperatureUnit>("second unit");
                            Console.WriteLine(TemperatureUnitExtensions.ConvertToBaseUnit(u1, v1) == TemperatureUnitExtensions.ConvertToBaseUnit(u2, v2) ? "Equal" : "Not Equal");
                            break;
                        case "4":
                            v1 = GetDouble("Enter value: "); u1 = SelectUnit<TemperatureUnit>("current unit");
                            target = SelectUnit<TemperatureUnit>("target unit");
                            Console.WriteLine($"Converted: {TemperatureUnitExtensions.ConvertFromBaseUnit(target, TemperatureUnitExtensions.ConvertToBaseUnit(u1, v1))} {target}");
                            break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
        }

        private double ConvertToBase<U>(U unit, double value)
        {
            return typeof(U) == typeof(LengthUnit)  ? LengthUnitExtensions.ConvertToBaseUnit((LengthUnit)(object)unit, value) :
                   typeof(U) == typeof(WeightUnit)  ? WeightUnitExtensions.ConvertToBaseUnit((WeightUnit)(object)unit, value) :
                   VolumeUnitExtensions.ConvertToBaseUnit((VolumeUnit)(object)unit, value);
        }

        private double ConvertFromBase<U>(U unit1, double value, U target)
        {
            return typeof(U) == typeof(LengthUnit)  ? LengthUnitExtensions.ConvertFromBaseUnit((LengthUnit)(object)target, ConvertToBase((LengthUnit)(object)unit1, value)) :
                   typeof(U) == typeof(WeightUnit)  ? WeightUnitExtensions.ConvertFromBaseUnit((WeightUnit)(object)target, ConvertToBase((WeightUnit)(object)unit1, value)) :
                   VolumeUnitExtensions.ConvertFromBaseUnit((VolumeUnit)(object)target, ConvertToBase((VolumeUnit)(object)unit1, value));
        }

        private double ConvertFromBase<U>(U unit1, U unit2, double v1, double v2, U target, string op)
        {
            double result = op == "+" ? ConvertToBase(unit1, v1) + ConvertToBase(unit2, v2)
                                      : ConvertToBase(unit1, v1) - ConvertToBase(unit2, v2);
            return typeof(U) == typeof(LengthUnit)  ? LengthUnitExtensions.ConvertFromBaseUnit((LengthUnit)(object)target, result) :
                   typeof(U) == typeof(WeightUnit)  ? WeightUnitExtensions.ConvertFromBaseUnit((WeightUnit)(object)target, result) :
                   VolumeUnitExtensions.ConvertFromBaseUnit((VolumeUnit)(object)target, result);
        }

        private double GetDouble(string prompt) { Console.Write(prompt); return double.Parse(Console.ReadLine()!); }

        private T SelectUnit<T>(string prompt) where T : struct, Enum
        {
            Console.WriteLine($"Select {prompt}:");
            var values = Enum.GetValues(typeof(T));
            int i = 1; foreach (var val in values) { Console.WriteLine($"{i}. {val}"); i++; }
            int choice = int.Parse(Console.ReadLine()!);
            if (choice < 1 || choice > values.Length) throw new Exception("Invalid unit choice.");
            return (T)values.GetValue(choice - 1)!;
        }
    }
}