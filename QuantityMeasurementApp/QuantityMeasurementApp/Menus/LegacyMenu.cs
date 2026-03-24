using QM.Models.Models;
using QM.BusinessLogic.Service;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Menus
{
    public class LegacyMenu
    {
        // ── Entry point called from Program.cs ──────────────────────────────
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
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "5") break;

                try
                {
                    switch (choice)
                    {
                        case "1": RunLengthMenu();      break;
                        case "2": RunWeightMenu();      break;
                        case "3": RunVolumeMenu();      break;
                        case "4": RunTemperatureMenu(); break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                }
            }
        }

        // ── Length ───────────────────────────────────────────────────────────
        // QuantityLength has: Add(other), ConvertTo(unit), Equals(obj)
        // LengthService has:  AreEqual, Convert, Add (with target unit)
        private static void RunLengthMenu()
        {
            while (true)
            {
                Console.WriteLine("\nLength Operations:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Subtract");
                Console.WriteLine("3. Divide");
                Console.WriteLine("4. Compare");
                Console.WriteLine("5. Convert");
                Console.WriteLine("6. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "6") break;

                try
                {
                    switch (choice)
                    {
                        case "1": LengthAdd();      break;
                        case "2": LengthSubtract(); break;
                        case "3": LengthDivide();   break;
                        case "4": LengthCompare();  break;
                        case "5": LengthConvert();  break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                }
            }
        }

        private static void LengthAdd()
        {
            var q1     = ReadQuantityLength("first");
            var q2     = ReadQuantityLength("second");
            var unit   = SelectLengthUnit("Select unit to convert sum to");
            // LengthService.Add(a, b, targetUnit) handles cross-unit addition
            var result = LengthService.Add(q1, q2, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        private static void LengthSubtract()
        {
            var q1 = ReadQuantityLength("first");
            var q2 = ReadQuantityLength("second");
            var unit = SelectLengthUnit("Select unit to convert result to");
            // Convert both to the target unit, then subtract
            double v1     = q1.ConvertTo(unit).Value;
            double v2     = q2.ConvertTo(unit).Value;
            double value  = v1 - v2;
            Console.WriteLine($"\nResult: {value} {unit}");
        }

        private static void LengthDivide()
        {
            var q1 = ReadQuantityLength("numerator");
            var q2 = ReadQuantityLength("denominator");
            // Convert both to FEET for a common base before dividing
            double v1     = q1.ConvertTo(LengthUnit.FEET).Value;
            double v2     = q2.ConvertTo(LengthUnit.FEET).Value;
            if (v2 == 0) throw new Exception("Cannot divide by zero.");
            double result = v1 / v2;
            Console.WriteLine($"\nResult: {result}");
        }

        private static void LengthCompare()
        {
            var q1 = ReadQuantityLength("first");
            var q2 = ReadQuantityLength("second");
            Console.WriteLine(LengthService.AreEqual(q1, q2)
                ? $"\n✔ {q1.Value} {q1.Unit} == {q2.Value} {q2.Unit} → EQUAL"
                : $"\n✔ {q1.Value} {q1.Unit} != {q2.Value} {q2.Unit} → NOT EQUAL");
        }

        private static void LengthConvert()
        {
            var source = ReadQuantityLength("source");
            var unit   = SelectLengthUnit("Select target unit");
            var result = LengthService.Convert(source, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        // ── Weight ───────────────────────────────────────────────────────────
        // WeightService has: AreEqual only
        // Use Quantity<WeightUnit> for Add/Subtract/Divide/Convert
        private static void RunWeightMenu()
        {
            while (true)
            {
                Console.WriteLine("\nWeight Operations:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Subtract");
                Console.WriteLine("3. Divide");
                Console.WriteLine("4. Compare");
                Console.WriteLine("5. Convert");
                Console.WriteLine("6. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "6") break;

                try
                {
                    switch (choice)
                    {
                        case "1": WeightAdd();      break;
                        case "2": WeightSubtract(); break;
                        case "3": WeightDivide();   break;
                        case "4": WeightCompare();  break;
                        case "5": WeightConvert();  break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                }
            }
        }

        private static void WeightAdd()
        {
            var w1     = ReadQuantityWeightGeneric("first");
            var w2     = ReadQuantityWeightGeneric("second");
            var unit   = SelectWeightUnit("Select unit to convert sum to");
            var result = w1.Add(w2, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        private static void WeightSubtract()
        {
            var w1     = ReadQuantityWeightGeneric("first");
            var w2     = ReadQuantityWeightGeneric("second");
            var unit   = SelectWeightUnit("Select unit to convert result to");
            var result = w1.Subtract(w2, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        private static void WeightDivide()
        {
            var w1     = ReadQuantityWeightGeneric("numerator");
            var w2     = ReadQuantityWeightGeneric("denominator");
            double result = w1.Divide(w2);
            Console.WriteLine($"\nResult: {result}");
        }

        private static void WeightCompare()
        {
            // WeightService.AreEqual works with QuantityWeight (legacy model)
            var w1 = ReadQuantityWeight("first");
            var w2 = ReadQuantityWeight("second");
            var service = new WeightService();
            Console.WriteLine(service.AreEqual(w1, w2)
                ? $"\n✔ {w1.Value} {w1.Unit} == {w2.Value} {w2.Unit} → EQUAL"
                : $"\n✔ {w1.Value} {w1.Unit} != {w2.Value} {w2.Unit} → NOT EQUAL");
        }

        private static void WeightConvert()
        {
            var source = ReadQuantityWeightGeneric("source");
            var unit   = SelectWeightUnit("Select target unit");
            var result = source.ConvertTo(unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        // ── Volume ───────────────────────────────────────────────────────────
        private static void RunVolumeMenu()
        {
            while (true)
            {
                Console.WriteLine("\nVolume Operations:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Subtract");
                Console.WriteLine("3. Divide");
                Console.WriteLine("4. Compare");
                Console.WriteLine("5. Convert");
                Console.WriteLine("6. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "6") break;

                try
                {
                    switch (choice)
                    {
                        case "1": VolumeAdd();      break;
                        case "2": VolumeSubtract(); break;
                        case "3": VolumeDivide();   break;
                        case "4": VolumeCompare();  break;
                        case "5": VolumeConvert();  break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                }
            }
        }

        private static void VolumeAdd()
        {
            var v1     = ReadQuantityVolume("first");
            var v2     = ReadQuantityVolume("second");
            var unit   = SelectVolumeUnit("Select unit to convert sum to");
            var result = v1.Add(v2, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        private static void VolumeSubtract()
        {
            var v1     = ReadQuantityVolume("first");
            var v2     = ReadQuantityVolume("second");
            var unit   = SelectVolumeUnit("Select unit to convert result to");
            var result = v1.Subtract(v2, unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        private static void VolumeDivide()
        {
            var v1     = ReadQuantityVolume("numerator");
            var v2     = ReadQuantityVolume("denominator");
            double result = v1.Divide(v2);
            Console.WriteLine($"\nResult: {result}");
        }

        private static void VolumeCompare()
        {
            var v1 = ReadQuantityVolume("first");
            var v2 = ReadQuantityVolume("second");
            Console.WriteLine(v1.Equals(v2)
                ? $"\n✔ {v1.Value} {v1.Unit} == {v2.Value} {v2.Unit} → EQUAL"
                : $"\n✔ {v1.Value} {v1.Unit} != {v2.Value} {v2.Unit} → NOT EQUAL");
        }

        private static void VolumeConvert()
        {
            var source = ReadQuantityVolume("source");
            var unit   = SelectVolumeUnit("Select target unit");
            var result = source.ConvertTo(unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        // ── Temperature ──────────────────────────────────────────────────────
        private static void RunTemperatureMenu()
        {
            while (true)
            {
                Console.WriteLine("\nTemperature Operations:");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("3. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "3") break;

                try
                {
                    switch (choice)
                    {
                        case "1": TemperatureCompare(); break;
                        case "2": TemperatureConvert(); break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                }
            }
        }

        private static void TemperatureCompare()
        {
            var t1 = ReadQuantityTemperature("first");
            var t2 = ReadQuantityTemperature("second");
            Console.WriteLine(t1.Equals(t2)
                ? $"\n✔ {t1.Value} {t1.Unit} == {t2.Value} {t2.Unit} → EQUAL"
                : $"\n✔ {t1.Value} {t1.Unit} != {t2.Value} {t2.Unit} → NOT EQUAL");
        }

        private static void TemperatureConvert()
        {
            var source = ReadQuantityTemperature("source");
            var unit   = SelectTemperatureUnit("Select target unit");
            var result = source.ConvertTo(unit);
            Console.WriteLine($"\nResult: {result.Value} {result.Unit}");
        }

        // ── Input helpers ────────────────────────────────────────────────────

        // Legacy QuantityLength model
        private static QuantityLength ReadQuantityLength(string label)
        {
            Console.Write($"Enter {label} value: ");
            double value    = double.Parse(Console.ReadLine()!);
            LengthUnit unit = SelectLengthUnit($"Select {label} unit");
            return new QuantityLength(value, unit);
        }

        private static LengthUnit SelectLengthUnit(string prompt)
        {
            Console.WriteLine($"\n{prompt}:");
            Console.WriteLine("1. INCHES");
            Console.WriteLine("2. FEET");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => LengthUnit.INCHES,
                "2" => LengthUnit.FEET,
                "3" => LengthUnit.YARDS,
                "4" => LengthUnit.CENTIMETERS,
                _   => throw new Exception("Invalid unit choice.")
            };
        }

        // Legacy QuantityWeight model (for WeightService.AreEqual)
        private static QuantityWeight ReadQuantityWeight(string label)
        {
            Console.Write($"Enter {label} value: ");
            double value    = double.Parse(Console.ReadLine()!);
            WeightUnit unit = SelectWeightUnit($"Select {label} unit");
            return new QuantityWeight(value, unit);
        }

        // Generic Quantity<WeightUnit> (for Add/Subtract/Divide/Convert)
        private static Quantity<WeightUnit> ReadQuantityWeightGeneric(string label)
        {
            Console.Write($"Enter {label} value: ");
            double value    = double.Parse(Console.ReadLine()!);
            WeightUnit unit = SelectWeightUnit($"Select {label} unit");
            return new Quantity<WeightUnit>(value, unit);
        }

        private static WeightUnit SelectWeightUnit(string prompt)
        {
            Console.WriteLine($"\n{prompt}:");
            Console.WriteLine("1. GRAM");
            Console.WriteLine("2. KILOGRAM");
            Console.WriteLine("3. POUND");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => WeightUnit.GRAM,
                "2" => WeightUnit.KILOGRAM,
                "3" => WeightUnit.POUND,
                _   => throw new Exception("Invalid unit choice.")
            };
        }

        private static Quantity<VolumeUnit> ReadQuantityVolume(string label)
        {
            Console.Write($"Enter {label} value: ");
            double value   = double.Parse(Console.ReadLine()!);
            VolumeUnit unit = SelectVolumeUnit($"Select {label} unit");
            return new Quantity<VolumeUnit>(value, unit);
        }

        private static VolumeUnit SelectVolumeUnit(string prompt)
        {
            Console.WriteLine($"\n{prompt}:");
            Console.WriteLine("1. MILLILITRE");
            Console.WriteLine("2. LITRE");
            Console.WriteLine("3. GALLON");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => VolumeUnit.MILLILITRE,
                "2" => VolumeUnit.LITRE,
                "3" => VolumeUnit.GALLON,
                _   => throw new Exception("Invalid unit choice.")
            };
        }

        private static Quantity<TemperatureUnit> ReadQuantityTemperature(string label)
        {
            Console.Write($"Enter {label} value: ");
            double value      = double.Parse(Console.ReadLine()!);
            TemperatureUnit unit = SelectTemperatureUnit($"Select {label} unit");
            return new Quantity<TemperatureUnit>(value, unit);
        }

        private static TemperatureUnit SelectTemperatureUnit(string prompt)
        {
            Console.WriteLine($"\n{prompt}:");
            Console.WriteLine("1. CELSIUS");
            Console.WriteLine("2. FAHRENHEIT");
            Console.WriteLine("3. KELVIN");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => TemperatureUnit.CELSIUS,
                "2" => TemperatureUnit.FAHRENHEIT,
                "3" => TemperatureUnit.KELVIN,
                _   => throw new Exception("Invalid unit choice.")
            };
        }
    }
}