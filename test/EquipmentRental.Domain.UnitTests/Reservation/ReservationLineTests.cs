using EquipmentRental.Domain.Product;
using EquipmentRental.Domain.Reservation;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Reservation;

public class ReservationLineTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInputs()
    {
        // Arrange
        var productId = ProductId.Create(Guid.NewGuid()).Value;
        uint quantity = 1;

        // Act
        var result = ReservationLine.Create(productId, quantity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ProductId.Should().Be(productId);
        result.Value.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void Create_ShouldFail_WhenProductIdIsNull()
    {
        // Arrange
        ProductId? productId = null;
        uint quantity = 5;

        // Act
        var result = ReservationLine.Create(productId, quantity);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.ProductIdRequired);
    }

    [Fact]
    public void Create_ShouldFail_WhenQuantityIsNull()
    {
        // Arrange
        ProductId productId = ProductId.Create(Guid.NewGuid()).Value;
        uint? quantity = null;

        // Act
        var result = ReservationLine.Create(productId, quantity);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.QuantityRequired);
    }

    [Fact]
    public void Create_ShouldFail_WhenQuantityIsZero()
    {
        // Arrange
        ProductId productId = ProductId.Create(Guid.NewGuid()).Value;
        uint? quantity = 0;

        // Act
        var result = ReservationLine.Create(productId, quantity);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.QuantityMustBeMoreThanZero);
    }
}