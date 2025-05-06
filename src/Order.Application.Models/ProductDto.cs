using System.Text.Json.Serialization;

namespace Order.Application.Models
{
    public sealed class ProductDto
    {
        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("vat")]
        public decimal VAT { get; set; }
    }
}
