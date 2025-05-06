namespace Order.Domain.Models
{
    public sealed record Product
    {
        public required string ProductId { get; init; }
        public required string ProductName { get; init; }
        public decimal Price { get; init; }
        public decimal Vat { get; init; }
        public List<Ingredient>? Ingredients { get; init; }
    }
}
