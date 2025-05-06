namespace Order.Domain.Models
{
    public sealed class Ingredient
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
