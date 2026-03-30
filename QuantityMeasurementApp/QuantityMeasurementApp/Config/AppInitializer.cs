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
            // 1. serviceProvider
                    // This is an object of IServiceProvider
                    // It is the Dependency Injection container
                    // It contains all the services you registered earlier
            // 2. GetRequiredService<T>() is an extension method of IServiceProvider.
                    // It is used to fetch a service from the container
                    // If the service exists → returns it 
                    // If not → throws exception
            // 3. ILogger<NTierMenu> - A logger specifically for the NTierMenu class
            // 4. logger is of type: ILogger<NTierMenu>. hence serviceProvider.GetRequiredService<ILogger<NTierMenu>>(); returns ILogger<NTierMenu> obj.
            
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
