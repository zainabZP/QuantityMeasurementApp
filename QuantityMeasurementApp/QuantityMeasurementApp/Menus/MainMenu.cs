using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QM.BusinessLogic.Service;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Config;

namespace QuantityMeasurementApp.Menus
{
    public class MainMenu
    {
        private readonly LegacyMenu _legacyMenu;
        private readonly NTierMenu  _nTierMenu;

        public MainMenu()
        {
            LoggingConfig.Configure();

            var serviceProvider = ServiceConfig.Configure();

            var logger       = serviceProvider.GetRequiredService<ILogger<NTierMenu>>();
            var dbRepository = serviceProvider.GetRequiredService<IQuantityMeasurementRepository>();
            var dbController = serviceProvider.GetRequiredService<QuantityMeasurementController>();

            var cacheRepository = QuantityMeasurementCacheRepository.Instance;
            var cacheService    = new QuantityMeasurementServiceImpl(cacheRepository);
            var cacheController = new QuantityMeasurementController(cacheService);

            // Flush cache → DB on startup
            FlushCacheToDatabase(cacheRepository, dbRepository);

            _legacyMenu = new LegacyMenu();
            _nTierMenu  = new NTierMenu(cacheController, dbController, dbRepository, logger);
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════╗");
                Console.WriteLine("║   Quantity Measurement App UC17  ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine("1. UC1–UC14 Legacy Menu");
                Console.WriteLine("2. UC15–UC17 N-Tier Menu");
                Console.WriteLine("3. Exit");
                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1": _legacyMenu.Run(); break;
                    case "2": _nTierMenu.Run();  break;
                    case "3": return;
                    default:  Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ── Flush cache → DB on startup ──────────────────────────────────────
        private static void FlushCacheToDatabase(
            QuantityMeasurementCacheRepository cacheRepository,
            IQuantityMeasurementRepository     dbRepository)
        {
            var cached = cacheRepository.GetAll();

            if (cached.Count == 0)
            {
                Console.WriteLine("[UC17] Cache is empty — nothing to flush.");
                return;
            }

            Console.WriteLine($"[UC17] Flushing {cached.Count} cached record(s) to database...");

            int saved = 0, skipped = 0;

            foreach (var entity in cached)
            {
                try
                {
                    dbRepository.Save(entity);
                    saved++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UC17] ⚠ Could not save {entity.Id}: {ex.Message}");
                    skipped++;
                }
            }

            cacheRepository.Clear();
            Console.WriteLine($"[UC17] Flush done — saved: {saved}, skipped: {skipped}, cache cleared.");
        }
    }
}