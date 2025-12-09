namespace PizzaMail.Contracts
{
    public class OrderCreatedEvent
    {
        public required string EventId { get; set; }
        public required string EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public required OrderCreatedEventData Data { get; set; }
    }
}
