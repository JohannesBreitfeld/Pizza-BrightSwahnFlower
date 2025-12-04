using FluentAssertions;
using Moq;
using OrderService.Core.Contracts;
using OrderService.Core.Domain;
using OrderService.Core.Features.CreateOrder;

namespace OrderService.Tests.Features.CreateOrder;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOutboxRepository> _mockOutboxRepository;
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _mockOutboxRepository = new Mock<IOutboxRepository>();
        _handler = new CreateOrderHandler(_mockOutboxRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Margherita Pizza",
                    Quantity = 2,
                    UnitPrice = 12.99m
                }
            }
        };

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.OrderId.Should().StartWith("ORD-");
        response.CustomerEmail.Should().Be("test@example.com");
        response.TotalAmount.Should().BeGreaterThan(0);
        response.TaxAmount.Should().BeGreaterThan(0);
        response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotalAmountCorrectly()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Pizza 1",
                    Quantity = 2,
                    UnitPrice = 10.00m
                },
                new OrderItemDto
                {
                    ProductId = "prod-002",
                    ProductName = "Pizza 2",
                    Quantity = 1,
                    UnitPrice = 15.00m
                }
            }
        };

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Subtotal: (2 * 10) + (1 * 15) = 35
        // Tax (25%): 35 * 0.25 = 8.75
        response.TotalAmount.Should().Be(35.00m);
        response.TaxAmount.Should().Be(8.75m);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectOrder()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Test Pizza",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        Order? capturedOrder = null;
        OutboxMessage? capturedOutbox = null;

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Callback<Order, OutboxMessage, CancellationToken>((order, outbox, ct) =>
            {
                capturedOrder = order;
                capturedOutbox = outbox;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockOutboxRepository.Verify(
            r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        capturedOrder.Should().NotBeNull();
        capturedOrder!.CustomerEmail.Should().Be("test@example.com");
        capturedOrder.Items.Should().HaveCount(1);
        capturedOrder.Items.First().ProductName.Should().Be("Test Pizza");
        capturedOrder.OrderId.Should().StartWith("ORD-");
        capturedOrder.Id.Should().Be(capturedOrder.OrderId); // Id and OrderId should match
    }

    [Fact]
    public async Task Handle_ShouldCreateOutboxMessageWithCorrectEventType()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Test Pizza",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        OutboxMessage? capturedOutbox = null;

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Callback<Order, OutboxMessage, CancellationToken>((order, outbox, ct) =>
            {
                capturedOutbox = outbox;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedOutbox.Should().NotBeNull();
        capturedOutbox!.EventType.Should().Be("OrderCreated");
        capturedOutbox.IsProcessed.Should().BeFalse();
        capturedOutbox.RetryCount.Should().Be(0);
        capturedOutbox.Payload.Should().NotBeNullOrEmpty();
        capturedOutbox.Payload.Should().Contain("OrderCreated");
        capturedOutbox.Payload.Should().Contain("test@example.com");
    }

    [Fact]
    public async Task Handle_ShouldEnsureOrderIdAndOutboxOrderIdMatch()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Test Pizza",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        Order? capturedOrder = null;
        OutboxMessage? capturedOutbox = null;

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Callback<Order, OutboxMessage, CancellationToken>((order, outbox, ct) =>
            {
                capturedOrder = order;
                capturedOutbox = outbox;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedOrder.Should().NotBeNull();
        capturedOutbox.Should().NotBeNull();
        capturedOrder!.OrderId.Should().Be(capturedOutbox!.OrderId);
    }

    [Fact]
    public async Task Handle_WithMultipleItems_ShouldCalculateCorrectly()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Margherita",
                    Quantity = 2,
                    UnitPrice = 12.50m
                },
                new OrderItemDto
                {
                    ProductId = "prod-002",
                    ProductName = "Pepperoni",
                    Quantity = 3,
                    UnitPrice = 15.00m
                },
                new OrderItemDto
                {
                    ProductId = "prod-003",
                    ProductName = "Hawaiian",
                    Quantity = 1,
                    UnitPrice = 13.99m
                }
            }
        };

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Subtotal: (2 * 12.50) + (3 * 15.00) + (1 * 13.99) = 25 + 45 + 13.99 = 83.99
        // Tax (25%): 83.99 * 0.25 = 20.9975
        var expectedSubtotal = 83.99m;
        var expectedTax = expectedSubtotal * 0.25m;

        response.TotalAmount.Should().Be(expectedSubtotal);
        response.TaxAmount.Should().Be(expectedTax);
    }

    [Fact]
    public async Task Handle_ShouldGenerateUniqueOrderIds()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "prod-001",
                    ProductName = "Pizza",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        _mockOutboxRepository
            .Setup(r => r.CreateWithOrderAsync(
                It.IsAny<Order>(),
                It.IsAny<OutboxMessage>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response1 = await _handler.Handle(command, CancellationToken.None);
        var response2 = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response1.OrderId.Should().NotBe(response2.OrderId);
    }
}
