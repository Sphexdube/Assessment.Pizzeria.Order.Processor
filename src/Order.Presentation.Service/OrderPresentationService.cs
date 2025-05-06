using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;

namespace Order.Presentation.Service;

public sealed class OrderPresentationService(ILogger logger, OrderProcessingService orderProcessingService)
{
    public async Task RunAsync(string[] args)
    {
        try
        {
            Print("======================================== Pizza Order Processing System ========================================");

            string filePath = args.Length > 0 ? args[0] : Path.Combine(Directory.GetCurrentDirectory(), "orders.json");

            Print($"\n Processing orders from: {filePath}\n----------------------------------------");

            OrderSummary summary = await orderProcessingService.ProcessOrdersAsync(filePath);

            DisplaySummary(summary);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred while processing orders", ex);

            Print($"Error: {ex.Message}");
        }

        Print("\n Press any key to exit...");

        Console.ReadKey();
    }

    private static void DisplaySummary(OrderSummary summary)
    {
        Print("\n ======================================== Order Summary ======================================== ");

        Print($"\n Successfully Processed Orders: {summary.ValidOrders.Count}");

        if (summary.ValidOrders.Any())
        {
            Print("----------------------------------------");
            
            summary.ValidOrders.ForEach(order =>

            Print($"Order ID: {order.OrderId}, Delivery: {order.DeliverAt}, Address: {order.CustomerAddress}, Items: {order.Items.Count}, Total: ${order.TotalPrice:F2} (VAT: ${order.TotalVAT:F2})\n"));
        }

        if (summary.InvalidOrders.Any())
        {
            Print($"\nInvalid Orders: {summary.InvalidOrders.Count}\n----------------------------------------");
            
            summary.InvalidOrders.ForEach(order => 

            Print($"Order ID: {order.OrderId} Address: {order.CustomerAddress} Items: {order.Items.Count} ----------------------------------------"));
        }

        Print("\n ======================================== Required Ingredients ========================================");

        if (!summary.RequiredIngredients.Any())
        {
            Print("No ingredients required");
        }
        else
        {
            foreach (var (name, amount) in summary.RequiredIngredients)
            {
                Print($"{name}: {amount:F2}");
            }
        }
    }

    private static void Print(string text) 
    {
        Console.WriteLine(text);
    }
}
