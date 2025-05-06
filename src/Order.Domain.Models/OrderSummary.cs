namespace Order.Domain.Models
{
    public sealed record OrderSummary
    {
        public required List<Order> ValidOrders { get; init; }
        public required List<Order> InvalidOrders { get; init; }
        public required Dictionary<string, decimal> RequiredIngredients { get; init; }
    }
}
