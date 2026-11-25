using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductManufacturerTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string manufacturer = "Sony";

        // Act
        var result = ProductManufacturer.Create(manufacturer);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Manufacturer.Should().Be(manufacturer);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidManufacturer(string? manufacturer)
    {
        // Act
        var result = ProductManufacturer.Create(manufacturer);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ManufacturerRequired);
    }

    [Fact]
    public void Create_ShouldTrimManufacturer_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string manufacturer = "  Sony  ";

        // Act
        var result = ProductManufacturer.Create(manufacturer);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Manufacturer.Should().Be(manufacturer.Trim());
    }
}
