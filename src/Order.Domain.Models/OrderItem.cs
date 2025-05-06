namespace Order.Domain.Models
{
    public sealed record OrderItem
    {
        public required string ProductId { get; init; }
        public int Quantity { get; init; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ProductId) && Quantity > 0;
        }
    }
}
