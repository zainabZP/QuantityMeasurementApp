using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QM.BusinessLogic.Service;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Menus;

namespace QuantityMeasurementApp.Config
{
    public static class AppInitializer
    {
        public static void Initialize()
        {
            var serviceProvider = ServiceConfig.Configure();

            // Get logger properly
            var logger = serviceProvider.GetRequiredService<ILogger<NTierMenu>>();

            // DB objects
            var dbRepository = serviceProvider.GetRequiredService<IQuantityMeasurementRepository>();
            var dbController = serviceProvider.GetRequiredService<QuantityMeasurementController>();

            // Cache
            var cacheRepository = QuantityMeasurementCacheRepository.Instance;
            var cacheService = new QuantityMeasurementServiceImpl(cacheRepository);
            var cacheController = new QuantityMeasurementController(cacheService);

            // Menus
            var legacyMenu = new LegacyMenu();
            var nTierMenu = new NTierMenu(
                cacheController,
                dbController,
                dbRepository,
                logger   // 
            );

            RunMainMenu(legacyMenu, nTierMenu);
        }

        private static void RunMainMenu(LegacyMenu legacyMenu, NTierMenu nTierMenu)
        {
            while (true)
            {
                Console.WriteLine("\n1. UC1–UC14 Menu");
                Console.WriteLine("2. UC15 Menu");
                Console.WriteLine("3. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": legacyMenu.Run(); break;
                    case "2": nTierMenu.Run(); break;
                    case "3": return;
                }
            }
        }
    }
}
