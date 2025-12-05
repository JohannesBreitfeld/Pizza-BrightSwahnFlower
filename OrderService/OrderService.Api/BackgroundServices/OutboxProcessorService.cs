using OrderService.Core.Contracts;

namespace OrderService.Api.BackgroundServices;

public class OutboxProcessorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessorService> _logger;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);
    private const int MaxRetryCount = 5;

    public OutboxProcessorService(
        IServiceProvider serviceProvider,
        ILogger<OutboxProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(_pollingInterval, stoppingToken);
        }

        _logger.LogDebug("Outbox Processor Service stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

        _logger.LogDebug("Checking for unprocessed outbox messages...");
        var messages = await outboxRepository.GetUnprocessedMessagesAsync(batchSize: 10, cancellationToken);

        if (messages.Count == 0)
        {
            _logger.LogDebug("No unprocessed outbox messages found");
            return;
        }

        _logger.LogInformation("Processing {Count} outbox messages", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                await PublishWithRetryAsync(
                    messagePublisher,
                    message.EventType,
                    message.Payload,
                    message.RetryCount,
                    cancellationToken);

                await outboxRepository.MarkAsProcessedAsync(message.Id, message.OrderId, cancellationToken);

                _logger.LogInformation(
                    "Successfully published message {MessageId} for order {OrderId}",
                    message.Id,
                    message.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to process outbox message {MessageId} for order {OrderId}. Retry count: {RetryCount}/{MaxRetryCount}",
                    message.Id,
                    message.OrderId,
                    message.RetryCount,
                    MaxRetryCount);

                try
                {
                    await outboxRepository.IncrementRetryCountAsync(message.Id, message.OrderId, cancellationToken);
                }
                catch (Exception incrementEx)
                {
                    _logger.LogError(incrementEx, "Failed to increment retry count for message {MessageId}", message.Id);
                }

                if (message.RetryCount >= MaxRetryCount)
                {
                    _logger.LogCritical(
                        "Message {MessageId} for order {OrderId} has exceeded maximum retry attempts and requires manual intervention",
                        message.Id,
                        message.OrderId);
                }
            }
        }
    }

    private async Task PublishWithRetryAsync(
        IMessagePublisher publisher,
        string eventType,
        string payload,
        int currentRetryCount,
        CancellationToken cancellationToken)
    {
        var attempt = 0;
        var maxAttempts = 3;

        while (attempt < maxAttempts)
        {
            try
            {
                await publisher.PublishAsync(eventType, payload, cancellationToken);
                return; 
            }
            catch (Exception ex)
            {
                attempt++;

                if (attempt >= maxAttempts)
                {
                    throw; 
                }

                var delaySeconds = Math.Pow(2, attempt);
                _logger.LogWarning(
                    ex,
                    "Publish attempt {Attempt}/{MaxAttempts} failed. Retrying in {Delay} seconds...",
                    attempt,
                    maxAttempts,
                    delaySeconds);

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
            }
        }
    }
}
