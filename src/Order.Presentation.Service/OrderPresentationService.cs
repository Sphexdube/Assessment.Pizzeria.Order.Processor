using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;

namespace Order.Presentation.Service
{
    public class OrderPresentationService
    {
        private readonly OrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;

        public OrderPresentationService(OrderProcessingService orderProcessingService, ILogger logger)
        {
            _orderProcessingService = orderProcessingService;
            _logger = logger;
        }

        public async Task RunAsync(string[] args)
        {
            try
            {
                Console.WriteLine("========================================");
                Console.WriteLine("    Pizza Order Processing System");
                Console.WriteLine("========================================");

                string filePath;
                if (args.Length > 0)
                {
                    filePath = args[0];
                }
                else
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "orders.json");
                }

                Console.WriteLine($"\nProcessing orders from: {filePath}");
                Console.WriteLine("----------------------------------------");

                var summary = await _orderProcessingService.ProcessOrdersAsync(filePath);

                DisplaySummary(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing orders", ex);
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DisplaySummary(OrderSummary summary)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("             Order Summary");
            Console.WriteLine("========================================");

            // Valid Orders
            Console.WriteLine($"\nSuccessfully Processed Orders: {summary.ValidOrders.Count}");
            if (summary.ValidOrders.Count > 0)
            {
                Console.WriteLine("----------------------------------------");
                foreach (var order in summary.ValidOrders)
                {
                    Console.WriteLine($"Order ID: {order.OrderId}");
                    Console.WriteLine($"Delivery: {order.DeliverAt}");
                    Console.WriteLine($"Address: {order.CustomerAddress}");
                    Console.WriteLine($"Items: {order.Items.Count}");
                    Console.WriteLine($"Total Price: ${order.TotalPrice:F2} (VAT: ${order.TotalVAT:F2})");
                    Console.WriteLine("----------------------------------------");
                }
            }

            // Invalid Orders
            if (summary.InvalidOrders.Count > 0)
            {
                Console.WriteLine($"\nInvalid Orders: {summary.InvalidOrders.Count}");
                Console.WriteLine("----------------------------------------");
                foreach (var order in summary.InvalidOrders)
                {
                    Console.WriteLine($"Order ID: {order.OrderId}");
                    Console.WriteLine($"Address: {order.CustomerAddress}");
                    Console.WriteLine($"Items: {order.Items.Count}");
                    Console.WriteLine("----------------------------------------");
                }
            }

            // Required Ingredients
            Console.WriteLine("\n========================================");
            Console.WriteLine("       Required Ingredients");
            Console.WriteLine("========================================");

            if (!summary.RequiredIngredients.Any())
            {
                Console.WriteLine("No ingredients required");
            }
            else
            {
                foreach (var ingredient in summary.RequiredIngredients)
                {
                    Console.WriteLine($"{ingredient.Key}: {ingredient.Value:F2}");
                }
            }
        }
    }
}
