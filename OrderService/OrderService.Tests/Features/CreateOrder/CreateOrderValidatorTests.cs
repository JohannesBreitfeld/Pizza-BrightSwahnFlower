using FluentAssertions;
using OrderService.Core.Features.CreateOrder;

namespace OrderService.Tests.Features.CreateOrder;

public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator;

    public CreateOrderValidatorTests()
    {
        _validator = new CreateOrderValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldNotHaveValidationErrors()
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

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_WithEmptyEmail_ShouldHaveValidationError(string email)
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = email!,
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

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CustomerEmail" &&
                                            e.ErrorMessage == "Customer email is required");
    }

    [Fact]
    public async Task Validate_WithInvalidEmailFormat_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "not-an-email",
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

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CustomerEmail" &&
                                            e.ErrorMessage == "Invalid email format");
    }

    [Fact]
    public async Task Validate_WithEmptyItems_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>()
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items" &&
                                            e.ErrorMessage.Contains("at least one item"));
    }

    [Fact]
    public async Task Validate_WithInvalidProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerEmail = "test@example.com",
            Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    ProductId = "",
                    ProductName = "Pizza",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items[0].ProductId");
    }

    [Fact]
    public async Task Validate_WithInvalidProductName_ShouldHaveValidationError()
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
                    ProductName = "",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items[0].ProductName");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WithInvalidQuantity_ShouldHaveValidationError(int quantity)
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
                    Quantity = quantity,
                    UnitPrice = 10.00m
                }
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items[0].Quantity" &&
                                            e.ErrorMessage.Contains("greater than 0"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WithInvalidUnitPrice_ShouldHaveValidationError(decimal price)
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
                    UnitPrice = price
                }
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items[0].UnitPrice" &&
                                            e.ErrorMessage.Contains("greater than 0"));
    }

    [Fact]
    public async Task Validate_WithMultipleItems_AllValid_ShouldNotHaveValidationErrors()
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
                },
                new OrderItemDto
                {
                    ProductId = "prod-002",
                    ProductName = "Pepperoni Pizza",
                    Quantity = 1,
                    UnitPrice = 14.99m
                }
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
