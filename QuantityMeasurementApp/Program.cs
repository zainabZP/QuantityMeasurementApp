using QM.BusinessLogic.Interface;   
using QM.BusinessLogic.Service;     
using QM.Repository.Repository;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Menus;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main()
        {
            // ── Dependency Injection ──────────────────────
            var repository = QuantityMeasurementCacheRepository.Instance;
            var service    = new QuantityMeasurementServiceImpl(repository);
            var controller = new QuantityMeasurementController(service);

            // ── Menu instances ────────────────────────────
            var legacyMenu = new LegacyMenu();
            var nTierMenu  = new NTierMenu(controller);

            while (true)
            {
                Console.WriteLine("\n╔═══════════════════════════════════════╗");
                Console.WriteLine("║   Quantity Measurement Application    ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.WriteLine("1. UC1–UC14 Menu (Backward Compatible)");
                Console.WriteLine("2. UC15 Menu (N-Tier Architecture)");
                Console.WriteLine("3. Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "1": legacyMenu.Run();  break;
                    case "2": nTierMenu.Run();   break;
                    case "3": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
    }
}