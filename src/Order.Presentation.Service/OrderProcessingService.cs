using Order.Application.Models.Interfaces;
using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Parser.Interfaces;
using Order.Infrastructure.Persistence.Interfaces;

namespace Order.Presentation.Service;

public sealed class OrderProcessingService(
        IOrderFileParser orderFileParser,
        IProductRepository productRepository,
        IOrderQueue orderQueue,
        ILogger logger)
{
    public async Task<OrderSummary> ProcessOrdersAsync(string orderFilePath)
    {
        logger.LogInformation($"Starting to process orders from {orderFilePath}");

        List<Domain.Entities.Order> entries = await orderFileParser.ParseOrderFileAsync(orderFilePath);
        List<Product> products = await productRepository.GetProductsAsync();

        logger.LogInformation($"Parsed {entries.Count} order entries");

        List<Domain.Models.Order> validOrders = new List<Domain.Models.Order>();
        List<Domain.Models.Order> invalidOrders = new List<Domain.Models.Order>();
        Dictionary<string, decimal> ingredientTotals = new Dictionary<string, decimal>();

        foreach (var group in entries.GroupBy(e => e.OrderId))
        {
            List<OrderItem> orderItems = group.Select(e => new OrderItem { ProductId = e.ProductId, Quantity = e.Quantity }).ToList();

            Domain.Models.Order order = new Domain.Models.Order
            {
                OrderId = group.Key,
                DeliverAt = group.First().DeliverAt,
                CreatedAt = group.First().CreatedAt,
                CustomerAddress = group.First().CustomerAddress,
                Items = orderItems
            };

            if (order.IsValid() && order.Items.All(i => i.IsValid()))
            {
                order.CalculateTotals(products);
                validOrders.Add(order);
                
                AddIngredients(order, products, ingredientTotals);
                
                await orderQueue.EnqueueOrderAsync(order);
                
                logger.LogInformation($"Order {order.OrderId} processed successfully");
            }
            else
            {
                invalidOrders.Add(order);
                logger.LogWarning($"Order {order.OrderId} is invalid");
            }
        }

        return new OrderSummary
        {
            ValidOrders = validOrders,
            InvalidOrders = invalidOrders,
            RequiredIngredients = ingredientTotals
        };
    }

    private static void AddIngredients(Domain.Models.Order order, List<Product> products, Dictionary<string, decimal> totals)
    {
        foreach (var item in order.Items)
        {
            if (products.FirstOrDefault(p => p.ProductId == item.ProductId) is { Ingredients: var ingredients })
            {
                foreach (var ing in ingredients)
                {
                    var amount = ing.Amount * item.Quantity;
                    totals[ing.Name] = totals.TryGetValue(ing.Name, out var existing) ? existing + amount : amount;
                }
            }
        }
    }
}