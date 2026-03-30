using Microsoft.Extensions.Logging;
using QM.Models.DTOs;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementApp.Menus
{
    public class NTierMenu
    {
        private readonly QuantityMeasurementController _cacheController;
        private readonly QuantityMeasurementController _dbController;
        private readonly IQuantityMeasurementRepository _dbRepository;
        private readonly ILogger _logger;

        public NTierMenu(
            QuantityMeasurementController cacheController,
            QuantityMeasurementController dbController,
            IQuantityMeasurementRepository dbRepository,
            ILogger logger)
        {
            _cacheController = cacheController ?? throw new ArgumentNullException(nameof(cacheController));
            _dbController    = dbController    ?? throw new ArgumentNullException(nameof(dbController));
            _dbRepository    = dbRepository    ?? throw new ArgumentNullException(nameof(dbRepository));
            _logger          = logger          ?? throw new ArgumentNullException(nameof(logger));
        }

        // ── Entry point called from Program.cs ──────────────────────────────
        public void Run()
        {
            var cacheRepository = QuantityMeasurementCacheRepository.Instance;

            while (true)
            {
                Console.WriteLine("\n╔═════════════════════════════════════════╗");
                Console.WriteLine("║  UC15/UC16 — N-Tier Architecture Menu   ║");
                Console.WriteLine("╚═════════════════════════════════════════╝");
                _logger.LogInformation("Menu started");
                Console.WriteLine("\n1. Cache Repository");
                Console.WriteLine("2. Database Repository");
                Console.WriteLine("3. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "3") break;

                try
                {
                    switch (choice)
                    {
                        case "1": RunOperationsMainMenu("Cache (Memory)", _cacheController, cacheRepository); break;
                        case "2": RunOperationsMainMenu("Database",        _dbController,   _dbRepository);   break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                    _logger.LogError("Operation failed: {Message}", ex.Message);
                }
            }
        }

        // ── Category menu (Length / Weight / Volume / Temperature + repo ops) ──
        private void RunOperationsMainMenu(
            string storageType,
            QuantityMeasurementController controller,
            IQuantityMeasurementRepository? repository)
        {
            while (true)
            {
                Console.WriteLine($"\n╔═════════════════════════════════════════════╗");
                Console.WriteLine($"║   {storageType,40} │");
                Console.WriteLine("╚═════════════════════════════════════════════╝");
                Console.WriteLine("\n1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Volume");
                Console.WriteLine("4. Temperature");
                Console.WriteLine("5. View All Measurements");
                Console.WriteLine("6. Get Measurement Count");
                Console.WriteLine("7. Query by Operation Type");
                Console.WriteLine("8. Clear All Measurements");
                Console.WriteLine("0. Back");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1": RunOperationsMenu("Length",      new[] { "FEET", "INCHES", "YARDS", "CENTIMETERS" }, controller); break;
                        case "2": RunOperationsMenu("Weight",      new[] { "GRAM", "KILOGRAM", "POUND" },              controller); break;
                        case "3": RunOperationsMenu("Volume",      new[] { "MILLILITRE", "LITRE", "GALLON" },          controller); break;
                        case "4": RunOperationsMenu("Temperature", new[] { "CELSIUS", "FAHRENHEIT", "KELVIN" },        controller); break;
                        case "5": if (repository != null) DisplayAllMeasurements(repository);  break;
                        case "6": if (repository != null) DisplayMeasurementCount(repository); break;
                        case "7": if (repository != null) QueryByOperation(repository);        break;
                        case "8": if (repository != null) ClearAllMeasurements(repository);   break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                    _logger.LogError("Operation failed: {Message}", ex.Message);
                }
            }
        }

        // ── Operations menu (Compare / Convert / Add / Subtract / Divide) ────
        private void RunOperationsMenu(
            string category,
            string[] units,
            QuantityMeasurementController controller)
        {
            while (true)
            {
                Console.WriteLine($"\n── {category} Operations ──");
                Console.WriteLine("1. Compare");
                Console.WriteLine("2. Convert");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Subtract");
                Console.WriteLine("5. Divide");
                Console.WriteLine("6. Back");
                Console.Write("Choice: ");

                string? choice = Console.ReadLine();
                if (choice == "6") break;

                try
                {
                    switch (choice)
                    {
                        case "1": HandleCompare(controller, category, units);   break;
                        case "2": HandleConvert(controller, category, units);   break;
                        case "3": HandleAdd(controller, category, units);       break;
                        case "4": HandleSubtract(controller, category, units);  break;
                        case "5": HandleDivide(controller, category, units);    break;
                        default:  Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR] {ex.Message}");
                }
            }
        }

        // ── Operation handlers ───────────────────────────────────────────────
        private void HandleCompare(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("first",  category, units);
            var q2 = ReadDTO("second", category, units);

            var result = controller.PerformCompare(q1, q2);
            Console.WriteLine(result.Value == 1
                ? $"\n✔ {q1} == {q2} → EQUAL"
                : $"\n✔ {q1} != {q2} → NOT EQUAL");
        }

        private void HandleConvert(QuantityMeasurementController controller, string category, string[] units)
        {
            var source      = ReadDTO("source", category, units);
            string target   = SelectUnit("target unit", units);

            var result = controller.PerformConvert(source, target);
            Console.WriteLine($"\n✔ {source} → {result}");
        }

        private void HandleAdd(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1          = ReadDTO("first",  category, units);
            var q2          = ReadDTO("second", category, units);
            string target   = SelectUnit("result unit", units);

            var result = controller.PerformAdd(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} + {q2} = {result}");
        }

        private void HandleSubtract(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1          = ReadDTO("first",  category, units);
            var q2          = ReadDTO("second", category, units);
            string target   = SelectUnit("result unit", units);

            var result = controller.PerformSubtract(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} - {q2} = {result}");
        }

        private void HandleDivide(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("numerator",   category, units);
            var q2 = ReadDTO("denominator", category, units);

            var result = controller.PerformDivide(q1, q2);
            Console.WriteLine($"\n✔ {q1} ÷ {q2} = {result.Value}");
        }

        // ── Repository display helpers ───────────────────────────────────────
        private static void DisplayAllMeasurements(IQuantityMeasurementRepository repository)
        {
            var measurements = repository.GetAll();
            if (measurements.Count == 0)
            {
                Console.WriteLine("No measurements found.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     Stored Measurements                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            foreach (var m in measurements)
                Console.WriteLine($"  {m}");
        }

        private static void DisplayMeasurementCount(IQuantityMeasurementRepository repository)
        {
            int count = repository.GetTotalCount();
            Console.WriteLine($"\n✓ Total measurements: {count}");
        }

        private static void QueryByOperation(IQuantityMeasurementRepository repository)
        {
            Console.Write("Enter operation type (Compare/Convert/Add/Subtract/Divide): ");
            string operationType = Console.ReadLine() ?? "Compare";

            var results = repository.GetByOperationType(operationType);
            if (results.Count == 0)
            {
                Console.WriteLine($"No measurements found for operation: {operationType}");
                return;
            }

            Console.WriteLine($"\n✓ Found {results.Count} measurements for '{operationType}':");
            foreach (var m in results)
                Console.WriteLine($"  {m}");
        }

        private static void ClearAllMeasurements(IQuantityMeasurementRepository repository)
        {
            Console.Write("Are you sure you want to delete all measurements? (Y/N): ");
            string? confirm = Console.ReadLine();
            if (confirm?.ToUpper() == "Y")
            {
                repository.Clear();
                Console.WriteLine("✓ All measurements cleared.");
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }

        // ── Input helpers ────────────────────────────────────────────────────
        private static QuantityDTO ReadDTO(string label, string category, string[] units)
        {
            Console.Write($"Enter {label} value: ");
            double value = double.Parse(Console.ReadLine()!);
            string unit  = SelectUnit($"{label} unit", units);
            return new QuantityDTO(value, unit, category);
        }

        private static string SelectUnit(string prompt, string[] units)
        {
            Console.WriteLine($"Select {prompt}:");
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine($"  {i + 1}. {units[i]}");
            Console.Write("Choice: ");
            int choice = int.Parse(Console.ReadLine()!) - 1;
            if (choice < 0 || choice >= units.Length)
                throw new Exception("Invalid unit choice.");
            return units[choice];
        }
    }
}

