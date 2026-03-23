using QuantityMeasurementAppDB.Controllers;

namespace QuantityMeasurementAppDB.Menus
{
    public class NTierMenu
    {
        private readonly QuantityMeasurementController _controller;

        public NTierMenu(QuantityMeasurementController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public void Run()
        {
            Console.WriteLine("\nN-Tier menu - UC15/UC16 operations");
            Console.WriteLine("(Use main menu in Program.cs for database operations)");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
