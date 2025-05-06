using System.Text.Json.Serialization;

namespace Order.Application.Models
{
    public sealed class OrderFileEntry
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("deliverAt")]
        public DateTime DeliverAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("customerAddress")]
        public string CustomerAddress { get; set; } = string.Empty;
    }
}
