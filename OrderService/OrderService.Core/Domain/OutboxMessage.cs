using Newtonsoft.Json;

namespace OrderService.Core.Domain;

public class OutboxMessage
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("orderId")]
    public string OrderId { get; set; } = string.Empty;

    [JsonProperty("eventType")]
    public string EventType { get; set; } = string.Empty;

    [JsonProperty("payload")]
    public string Payload { get; set; } = string.Empty;

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty("isProcessed")]
    public bool IsProcessed { get; set; }

    [JsonProperty("processedAt")]
    public DateTime? ProcessedAt { get; set; }

    [JsonProperty("retryCount")]
    public int RetryCount { get; set; }

    [JsonProperty("type")]
    public string Type => "OutboxMessage";
}
