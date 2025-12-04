namespace OrderService.Core.Contracts;

public interface IMessagePublisher
{
    Task PublishAsync(string eventType, string message, CancellationToken cancellationToken = default);
}
