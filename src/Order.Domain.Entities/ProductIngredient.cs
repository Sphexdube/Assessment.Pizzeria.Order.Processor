namespace Order.Domain.Entities
{
    public sealed record ProductIngredient
    {
        public required string ProductId { get; init; }

        public required List<Ingredient> Ingredients { get; init; }
    }
}
