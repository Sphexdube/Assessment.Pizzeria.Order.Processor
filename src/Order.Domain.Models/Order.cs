namespace Order.Domain.Models
{
    public sealed class Order
    {
        public string OrderId { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public DateTime DeliverAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerAddress { get; set; } = string.Empty;
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
                decimal itemVAT = itemPrice * (product.VAT / 100);

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
