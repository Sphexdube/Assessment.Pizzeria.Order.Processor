using System.Text.Json.Serialization;

namespace Order.Application.Models
{
    public sealed class ProductIngredientDto
    {
        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("ingredients")]
        public List<IngredientDto> Ingredients { get; set; } = new();
    }
}
