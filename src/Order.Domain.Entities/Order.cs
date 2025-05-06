namespace Order.Domain.Entities
{
    public sealed class Order
    {
        public required string OrderId { get; init; }

        public required string ProductId { get; init; }

        public required int Quantity { get; init; }

        public required DateTime DeliverAt { get; init; }

        public required DateTime CreatedAt { get; init; }

        public required string CustomerAddress { get; init; }
    }
}
