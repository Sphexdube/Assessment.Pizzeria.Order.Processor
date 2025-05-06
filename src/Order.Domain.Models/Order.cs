namespace Order.Domain.Models
{
    public sealed record Order
    {
        public required string OrderId { get; init; }
        public required List<OrderItem> Items { get; init; }
        public required string CustomerAddress { get; init; }
        public DateTime DeliverAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public decimal TotalPrice { get; private set; }
        public decimal TotalVAT { get; private set; }

        public void CalculateTotals(IEnumerable<Product> products)
        {
            TotalPrice = 0;
            TotalVAT = 0;

            foreach (var item in Items)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null) continue;

                decimal itemPrice = product.Price * item.Quantity;
                decimal itemVAT = itemPrice * (product.Vat / 100);

                TotalPrice += itemPrice;
                TotalVAT += itemVAT;
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(OrderId)
                && Items.Count > 0
                && !string.IsNullOrEmpty(CustomerAddress)
                && DeliverAt > CreatedAt;
        }
    }
}
