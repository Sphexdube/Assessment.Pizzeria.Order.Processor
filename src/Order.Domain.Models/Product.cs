namespace Order.Domain.Models
{
    public sealed class Product
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
