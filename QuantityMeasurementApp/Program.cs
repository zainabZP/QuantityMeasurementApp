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
                var connectionString = "Data Source=QuantityMeasurement.db";
                services.AddDbContext<QuantityMeasurementDbContext>(options =>
                    options.UseSqlite(connectionString)
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
                Console.WriteLine("\n╔═══════════════════════════════════════════════════╗");
                Console.WriteLine("║   Quantity Measurement Application with Database   ║");
                Console.WriteLine("║            (UC16 - Database Integration)          ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════╝");
                Console.WriteLine("\n1. Compare Quantities");
                Console.WriteLine("2. Convert Quantity");
                Console.WriteLine("3. Add Quantities");
                Console.WriteLine("4. Subtract Quantities");
                Console.WriteLine("5. Divide Quantities");
                Console.WriteLine("6. View All Measurements (Database)");
                Console.WriteLine("7. Get Measurement Count");
                Console.WriteLine("8. Query by Operation Type");
                Console.WriteLine("9. Clear All Measurements");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": HandleCompare(controller); break;
                        case "2": HandleConvert(controller); break;
                        case "3": HandleAdd(controller); break;
                        case "4": HandleSubtract(controller); break;
                        case "5": HandleDivide(controller); break;
                        case "6": DisplayAllMeasurements(repository); break;
                        case "7": DisplayMeasurementCount(repository); break;
                        case "8": QueryByOperation(repository); break;
                        case "9": ClearAllMeasurements(repository); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
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

        static void HandleCompare(QuantityMeasurementController controller)
        {
            Console.Write("Enter first quantity value: ");
            double val1 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter first quantity unit (FEET/INCH/YARD/CM/etc): ");
            string unit1 = Console.ReadLine() ?? "FEET";
            Console.Write("Enter measurement type (Length/Weight/Volume/Temperature): ");
            string type1 = Console.ReadLine() ?? "Length";

            Console.Write("Enter second quantity value: ");
            double val2 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter second quantity unit: ");
            string unit2 = Console.ReadLine() ?? "FEET";

            var q1 = new QuantityDTO(val1, unit1, type1);
            var q2 = new QuantityDTO(val2, unit2, type1);
            var result = controller.PerformCompare(q1, q2);

            Console.WriteLine($"✓ Result: {result.Value} (Equal: {result.Value == 1})");
        }

        static void HandleConvert(QuantityMeasurementController controller)
        {
            Console.Write("Enter quantity value: ");
            double value = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter source unit: ");
            string sourceUnit = Console.ReadLine() ?? "FEET";
            Console.Write("Enter measurement type (Length/Weight/Volume/Temperature): ");
            string type = Console.ReadLine() ?? "Length";
            Console.Write("Enter target unit: ");
            string targetUnit = Console.ReadLine() ?? "INCH";

            var source = new QuantityDTO(value, sourceUnit, type);
            var result = controller.PerformConvert(source, targetUnit);

            Console.WriteLine($"✓ Converted: {source} = {result}");
        }

        static void HandleAdd(QuantityMeasurementController controller)
        {
            Console.Write("Enter first quantity value: ");
            double val1 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter first quantity unit: ");
            string unit1 = Console.ReadLine() ?? "FEET";
            Console.Write("Enter measurement type (Length/Weight/Volume): ");
            string type = Console.ReadLine() ?? "Length";

            Console.Write("Enter second quantity value: ");
            double val2 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter second quantity unit: ");
            string unit2 = Console.ReadLine() ?? "FEET";

            Console.Write("Enter target unit for result: ");
            string targetUnit = Console.ReadLine() ?? "FEET";

            var q1 = new QuantityDTO(val1, unit1, type);
            var q2 = new QuantityDTO(val2, unit2, type);
            var result = controller.PerformAdd(q1, q2, targetUnit);

            Console.WriteLine($"✓ Sum: {q1} + {q2} = {result}");
        }

        static void HandleSubtract(QuantityMeasurementController controller)
        {
            Console.Write("Enter first quantity value: ");
            double val1 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter first quantity unit: ");
            string unit1 = Console.ReadLine() ?? "FEET";
            Console.Write("Enter measurement type (Length/Weight/Volume): ");
            string type = Console.ReadLine() ?? "Length";

            Console.Write("Enter second quantity value: ");
            double val2 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter second quantity unit: ");
            string unit2 = Console.ReadLine() ?? "FEET";

            Console.Write("Enter target unit for result: ");
            string targetUnit = Console.ReadLine() ?? "FEET";

            var q1 = new QuantityDTO(val1, unit1, type);
            var q2 = new QuantityDTO(val2, unit2, type);
            var result = controller.PerformSubtract(q1, q2, targetUnit);

            Console.WriteLine($"✓ Difference: {q1} - {q2} = {result}");
        }

        static void HandleDivide(QuantityMeasurementController controller)
        {
            Console.Write("Enter first quantity value: ");
            double val1 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter first quantity unit: ");
            string unit1 = Console.ReadLine() ?? "FEET";
            Console.Write("Enter measurement type (Length/Weight/Volume): ");
            string type = Console.ReadLine() ?? "Length";

            Console.Write("Enter second quantity value: ");
            double val2 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter second quantity unit: ");
            string unit2 = Console.ReadLine() ?? "FEET";

            var q1 = new QuantityDTO(val1, unit1, type);
            var q2 = new QuantityDTO(val2, unit2, type);
            var result = controller.PerformDivide(q1, q2);

            Console.WriteLine($"✓ Quotient: {q1} / {q2} = {result.Value}");
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
