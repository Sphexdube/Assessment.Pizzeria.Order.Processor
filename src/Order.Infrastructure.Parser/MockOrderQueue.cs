using Order.Application.Models.Interfaces;
using Order.Domain.Observability.Interfaces;

namespace Order.Infrastructure.Parser
{
    public sealed class MockOrderQueue : IOrderQueue
    {
        private readonly ILogger _logger;
        private readonly List<Domain.Models.Order> _enqueuedOrders = new();

        public MockOrderQueue(ILogger logger)
        {
            _logger = logger;
        }

        public Task EnqueueOrderAsync(Domain.Models.Order order)
        {
            _logger.LogInformation($"Enqueuing order {order.OrderId}");
            _enqueuedOrders.Add(order);

            return Task.CompletedTask;
        }

        public List<Domain.Models.Order> GetEnqueuedOrders()
        {
            return _enqueuedOrders;
        }
    }
}
