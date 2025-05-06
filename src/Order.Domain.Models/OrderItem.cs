namespace Order.Domain.Models
{
    public sealed class OrderItem
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ProductId) && Quantity > 0;
        }
    }
}
