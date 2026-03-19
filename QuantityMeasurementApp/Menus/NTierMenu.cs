using QM.Models.DTOs;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementApp.Menus
{
    public class NTierMenu
    {
        private readonly QuantityMeasurementController _controller;

        public NTierMenu(QuantityMeasurementController controller)
        {
            _controller = controller;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════╗");
                Console.WriteLine("║  UC15 — N-Tier Architecture  ║");
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
                    case "1": RunOperationsMenu("Length",      new[] { "FEET", "INCHES", "YARDS", "CENTIMETERS" }); break;
                    case "2": RunOperationsMenu("Weight",      new[] { "GRAM", "KILOGRAM", "POUND" });              break;
                    case "3": RunOperationsMenu("Volume",      new[] { "MILLILITRE", "LITRE", "GALLON" });          break;
                    case "4": RunOperationsMenu("Temperature", new[] { "CELSIUS", "FAHRENHEIT", "KELVIN" });        break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        private void RunOperationsMenu(string category, string[] units)
        {
            while (true)
            {
                Console.WriteLine($"\n── {category} Operations ──");
                Console.WriteLine("1. Compare\n2. Convert\n3. Add\n4. Subtract\n5. Divide\n6. Back");
                string choice = Console.ReadLine()!;
                if (choice == "6") break;

                try
                {
                    switch (choice)
                    {
                        case "1": HandleCompare(category, units);   break;
                        case "2": HandleConvert(category, units);   break;
                        case "3": HandleAdd(category, units);       break;
                        case "4": HandleSubtract(category, units);  break;
                        case "5": HandleDivide(category, units);    break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR] {ex.Message}");
                }
            }
        }

        private void HandleCompare(string category, string[] units)
        {
            var q1 = ReadDTO("first",  category, units);
            var q2 = ReadDTO("second", category, units);

            var result = _controller.PerformCompare(q1, q2);
            Console.WriteLine(result.Value == 1
                ? $"\n✔ {q1} == {q2} → EQUAL"
                : $"\n✔ {q1} != {q2} → NOT EQUAL");
        }

        private void HandleConvert(string category, string[] units)
        {
            var source    = ReadDTO("source", category, units);
            string target = SelectUnit("target unit", units);

            var result = _controller.PerformConvert(source, target);
            Console.WriteLine($"\n✔ {source} → {result}");
        }

        private void HandleAdd(string category, string[] units)
        {
            var q1        = ReadDTO("first",  category, units);
            var q2        = ReadDTO("second", category, units);
            string target = SelectUnit("result unit", units);

            var result = _controller.PerformAdd(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} + {q2} = {result}");
        }

        private void HandleSubtract(string category, string[] units)
        {
            var q1        = ReadDTO("first",  category, units);
            var q2        = ReadDTO("second", category, units);
            string target = SelectUnit("result unit", units);

            var result = _controller.PerformSubtract(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} - {q2} = {result}");
        }

        private void HandleDivide(string category, string[] units)
        {
            var q1 = ReadDTO("numerator",   category, units);
            var q2 = ReadDTO("denominator", category, units);

            var result = _controller.PerformDivide(q1, q2);
            Console.WriteLine($"\n✔ {q1} ÷ {q2} = {result.Value}");
        }

        // ── Helpers ──────────────────────────────
        private QuantityDTO ReadDTO(string label, string category, string[] units)
        {
            Console.Write($"Enter {label} value: ");
            double value  = double.Parse(Console.ReadLine()!);
            string unit   = SelectUnit($"{label} unit", units);
            return new QuantityDTO(value, unit, category);
        }

        private string SelectUnit(string prompt, string[] units)
        {
            Console.WriteLine($"Select {prompt}:");
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine($"  {i + 1}. {units[i]}");
            int choice = int.Parse(Console.ReadLine()!) - 1;
            if (choice < 0 || choice >= units.Length)
                throw new Exception("Invalid unit choice.");
            return units[choice];
        }
    }
}