using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class RentalRateTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        decimal rate = 25.00m;
        PricingBasis pricingBasis = PricingBasis.Daily;

        // Act
        var result = RentalRate.Create(rate, pricingBasis);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Rate.Should().Be(rate);
        result.Value.PricingBasis.Should().Be(pricingBasis);
    }

    [Fact]
    public void Create_ShouldFail_WithNullRate()
    {
        // Arrange
        PricingBasis pricingBasis = PricingBasis.Daily;

        // Act
        var result = RentalRate.Create(null, pricingBasis);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.RateRequired);
    }

    [Fact]
    public void Create_ShouldFail_WithZeroRate()
    {
        // Arrange
        decimal rate = 0m;
        PricingBasis pricingBasis = PricingBasis.Daily;

        // Act
        var result = RentalRate.Create(rate, pricingBasis);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.RateRequired);
    }

    [Fact]
    public void Create_ShouldFail_WithNegativeRate()
    {
        // Arrange
        decimal rate = -10m;
        PricingBasis pricingBasis = PricingBasis.Daily;

        // Act
        var result = RentalRate.Create(rate, pricingBasis);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.RateRequired);
    }

    [Fact]
    public void Create_ShouldFail_WithNullPricingBasis()
    {
        // Arrange
        decimal rate = 25.00m;
        PricingBasis? pricingBasis = null;

        // Act
        var result = RentalRate.Create(rate, pricingBasis);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.PricingBasisRequired);
    }
}
