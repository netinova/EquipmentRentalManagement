using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductDescriptionTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string description = "A high-quality camera.";

        // Act
        var result = ProductDescription.Create(description);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidDescription(string? description)
    {
        // Act
        var result = ProductDescription.Create(description);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.DescriptionRequired);
    }

    [Fact]
    public void Create_ShouldFail_WhenDescriptionExceeds100Characters()
    {
        // Arrange
        string description = new('a', 101);

        // Act
        var result = ProductDescription.Create(description);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.DescriptionTooLong);
    }

    [Fact]
    public void Create_ShouldTrimDescription_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string description = "  A high-quality power drill.  ";

        // Act
        var result = ProductDescription.Create(description);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Description.Should().Be(description.Trim());
    }
}
