using Order.Application.Models.Interfaces;
using Order.Domain.Models;
using Order.Domain.Observability.Interfaces;
using Order.Infrastructure.Parser.Interfaces;

namespace Order.Presentation.Service
{
    public class OrderProcessingService
    {
        private readonly IOrderFileParser _orderFileParser;
        private readonly IProductRepository _productRepository;
        private readonly IOrderQueue _orderQueue;
        private readonly ILogger _logger;

        public OrderProcessingService(
            IOrderFileParser orderFileParser,
            IProductRepository productRepository,
            IOrderQueue orderQueue,
            ILogger logger)
        {
            _orderFileParser = orderFileParser;
            _productRepository = productRepository;
            _orderQueue = orderQueue;
            _logger = logger;
        }

        public async Task<OrderSummary> ProcessOrdersAsync(string orderFilePath)
        {
            _logger.LogInformation($"Starting to process orders from {orderFilePath}");

            var orderEntries = await _orderFileParser.ParseOrderFileAsync(orderFilePath);
            var products = await _productRepository.GetProductsAsync();

            _logger.LogInformation($"Parsed {orderEntries.Count} order entries");

            // Group entries by OrderId
            var orderGroups = orderEntries.GroupBy(e => e.OrderId);

            var validOrders = new List<Domain.Models.Order>();
            var invalidOrders = new List<Domain.Models.Order>();
            var ingredientTotals = new Dictionary<string, decimal>();

            foreach (var group in orderGroups)
            {
                var orderEntryGroup = group.ToList();
                if (orderEntryGroup.Count == 0) continue;

                // Create order from the first entry
                var firstEntry = orderEntryGroup.First();
                var order = new Domain.Models.Order
                {
                    OrderId = firstEntry.OrderId,
                    DeliverAt = firstEntry.DeliverAt,
                    CreatedAt = firstEntry.CreatedAt,
                    CustomerAddress = firstEntry.CustomerAddress,
                    Items = new List<OrderItem>()
                };

                // Add all items from the group
                foreach (var entry in orderEntryGroup)
                {
                    order.Items.Add(new OrderItem
                    {
                        ProductId = entry.ProductId,
                        Quantity = entry.Quantity
                    });
                }

                // Validate order
                if (order.IsValid() && order.Items.All(i => i.IsValid()))
                {
                    // Calculate totals
                    order.CalculateTotals(products);

                    // Add to valid orders
                    validOrders.Add(order);

                    // Calculate required ingredients
                    CalculateIngredients(order, products, ingredientTotals);

                    // Enqueue order
                    await _orderQueue.EnqueueOrderAsync(order);

                    _logger.LogInformation($"Order {order.OrderId} processed successfully");
                }
                else
                {
                    invalidOrders.Add(order);
                    _logger.LogWarning($"Order {order.OrderId} is invalid");
                }
            }

            return new OrderSummary
            {
                ValidOrders = validOrders,
                InvalidOrders = invalidOrders,
                RequiredIngredients = ingredientTotals
            };
        }

        private void CalculateIngredients(Domain.Models.Order order, List<Product> products, Dictionary<string, decimal> ingredientTotals)
        {
            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null) continue;

                foreach (var ingredient in product.Ingredients)
                {
                    var totalAmount = ingredient.Amount * item.Quantity;

                    if (ingredientTotals.ContainsKey(ingredient.Name))
                    {
                        ingredientTotals[ingredient.Name] += totalAmount;
                    }
                    else
                    {
                        ingredientTotals[ingredient.Name] = totalAmount;
                    }
                }
            }
        }
    }
}
