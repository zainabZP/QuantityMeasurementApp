using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;
using QM.Models.DTOs;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // ── Configure Serilog ──────────────────────────
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Application starting...");

                // ── Setup Dependency Injection ──────────────
                var services = new ServiceCollection();
                
                // Configure DbContext
                var connectionString = "Data Source=WASEEM\\SQLEXPRESS;Initial Catalog=QuantityMeasurementDb;Integrated Security=true;TrustServerCertificate=true;";
                services.AddDbContext<QuantityMeasurementDbContext>(options =>
                    options.UseSqlServer(connectionString)
                );

                // Register services
                services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
                services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
                services.AddScoped<QuantityMeasurementController>();
                services.AddLogging(config =>
                {
                    config.AddSerilog();
                });

                var serviceProvider = services.BuildServiceProvider();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                // ── Initialize Database ────────────────────
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
                    logger.LogInformation("Initializing database...");
                    await dbContext.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database initialized successfully");
                }

                // ── DI Resolution ──────────────────────────
                var repository = serviceProvider.GetRequiredService<IQuantityMeasurementRepository>();
                var service = serviceProvider.GetRequiredService<IQuantityMeasurementService>();
                var controller = serviceProvider.GetRequiredService<QuantityMeasurementController>();

                logger.LogInformation("Dependency Injection complete. Application ready.");

                // ── Run Application Menu ────────────────────
                RunMenu(controller, repository, logger);

                logger.LogInformation("Application completed successfully");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static void RunMenu(QuantityMeasurementController controller, IQuantityMeasurementRepository repository, Microsoft.Extensions.Logging.ILogger logger)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n╔═════════════════════════════════════════════╗");
                Console.WriteLine("║   Quantity Measurement Application with DB   ║");
                Console.WriteLine("║       (UC16 - Database Integration)         ║");
                Console.WriteLine("╚═════════════════════════════════════════════╝");
                Console.WriteLine("\n1. Length");
                Console.WriteLine("2. Weight");
                Console.WriteLine("3. Volume");
                Console.WriteLine("4. Temperature");
                Console.WriteLine("5. View All Measurements (Database)");
                Console.WriteLine("6. Get Measurement Count");
                Console.WriteLine("7. Query by Operation Type");
                Console.WriteLine("8. Clear All Measurements");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": RunOperationsMenu("Length", new[] { "FEET", "INCHES", "YARDS", "CENTIMETERS" }, controller); break;
                        case "2": RunOperationsMenu("Weight", new[] { "GRAM", "KILOGRAM", "POUND" }, controller); break;
                        case "3": RunOperationsMenu("Volume", new[] { "MILLILITRE", "LITRE", "GALLON" }, controller); break;
                        case "4": RunOperationsMenu("Temperature", new[] { "CELSIUS", "FAHRENHEIT", "KELVIN" }, controller); break;
                        case "5": DisplayAllMeasurements(repository); break;
                        case "6": DisplayMeasurementCount(repository); break;
                        case "7": QueryByOperation(repository); break;
                        case "8": ClearAllMeasurements(repository); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                    logger.LogError($"Operation failed: {ex.Message}");
                }
            }

            Console.WriteLine("\nThank you for using Quantity Measurement Application!");
        }

        static void RunOperationsMenu(string category, string[] units, QuantityMeasurementController controller)
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
                        case "1": HandleCompare(controller, category, units); break;
                        case "2": HandleConvert(controller, category, units); break;
                        case "3": HandleAdd(controller, category, units); break;
                        case "4": HandleSubtract(controller, category, units); break;
                        case "5": HandleDivide(controller, category, units); break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR] {ex.Message}");
                }
            }
        }

        static QuantityDTO ReadDTO(string label, string category, string[] units)
        {
            Console.Write($"Enter {label} value: ");
            double value = double.Parse(Console.ReadLine()!);
            string unit = SelectUnit($"{label} unit", units);
            return new QuantityDTO(value, unit, category);
        }

        static string SelectUnit(string prompt, string[] units)
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

        static void HandleCompare(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("first", category, units);
            var q2 = ReadDTO("second", category, units);

            var result = controller.PerformCompare(q1, q2);
            Console.WriteLine(result.Value == 1
                ? $"\n✔ {q1} == {q2} → EQUAL"
                : $"\n✔ {q1} != {q2} → NOT EQUAL");
        }

        static void HandleConvert(QuantityMeasurementController controller, string category, string[] units)
        {
            var source = ReadDTO("source", category, units);
            string target = SelectUnit("target unit", units);

            var result = controller.PerformConvert(source, target);
            Console.WriteLine($"\n✔ {source} → {result}");
        }

        static void HandleAdd(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("first", category, units);
            var q2 = ReadDTO("second", category, units);
            string target = SelectUnit("result unit", units);

            var result = controller.PerformAdd(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} + {q2} = {result}");
        }

        static void HandleSubtract(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("first", category, units);
            var q2 = ReadDTO("second", category, units);
            string target = SelectUnit("result unit", units);

            var result = controller.PerformSubtract(q1, q2, target);
            Console.WriteLine($"\n✔ {q1} - {q2} = {result}");
        }

        static void HandleDivide(QuantityMeasurementController controller, string category, string[] units)
        {
            var q1 = ReadDTO("numerator", category, units);
            var q2 = ReadDTO("denominator", category, units);

            var result = controller.PerformDivide(q1, q2);
            Console.WriteLine($"\n✔ {q1} ÷ {q2} = {result.Value}");
        }

        static void DisplayAllMeasurements(IQuantityMeasurementRepository repository)
        {
            var measurements = repository.GetAll();
            if (measurements.Count == 0)
            {
                Console.WriteLine("No measurements found in database.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     Stored Measurements                               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            foreach (var m in measurements)
            {
                Console.WriteLine($"  {m}");
            }
        }

        static void DisplayMeasurementCount(IQuantityMeasurementRepository repository)
        {
            int count = repository.GetTotalCount();
            Console.WriteLine($"\n✓ Total measurements in database: {count}");
        }

        static void QueryByOperation(IQuantityMeasurementRepository repository)
        {
            Console.Write("Enter operation type (Compare/Convert/Add/Subtract/Divide): ");
            string operationType = Console.ReadLine() ?? "Compare";

            var results = repository.GetByOperationType(operationType);
            if (results.Count == 0)
            {
                Console.WriteLine($"No measurements found for operation: {operationType}");
                return;
            }

            Console.WriteLine($"\n✓ Found {results.Count} measurements for operation '{operationType}':");
            foreach (var m in results)
            {
                Console.WriteLine($"  {m}");
            }
        }

        static void ClearAllMeasurements(IQuantityMeasurementRepository repository)
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

        // ── Legacy UC2 Helper Methods for Backward Compatibility ──
        public static bool CheckFeetEquality(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 0.001;
        }

        public static bool CheckInchEquality(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 0.001;
        }
    }
}
