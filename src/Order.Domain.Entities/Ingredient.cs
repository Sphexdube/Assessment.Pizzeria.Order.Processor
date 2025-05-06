namespace Order.Domain.Entities
{
    public sealed class Ingredient
    {
        public required string Name { get; init; }

        public required decimal Amount { get; init; }

        public required string Unit { get; init; }
    }
}
