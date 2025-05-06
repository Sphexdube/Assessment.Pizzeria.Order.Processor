namespace Order.Domain.Models
{
    public sealed class OrderSummary
    {
        public List<Order> ValidOrders { get; set; } = new();
        public List<Order> InvalidOrders { get; set; } = new();
        public Dictionary<string, decimal> RequiredIngredients { get; set; } = new();
    }
}
