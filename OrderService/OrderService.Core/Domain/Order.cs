using Newtonsoft.Json;

namespace OrderService.Core.Domain;

public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("orderId")]
    public string OrderId { get; set; } = string.Empty;

    [JsonProperty("customerEmail")]
    public string CustomerEmail { get; set; } = string.Empty;

    [JsonProperty("items")]
    public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();

    [JsonProperty("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonProperty("taxAmount")]
    public decimal TaxAmount { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty("type")]
    public string Type => "Order";
}
