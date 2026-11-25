using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductCategoryTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string category = "Power Tools";

        // Act
        var result = ProductCategory.Create(category);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Category.Should().Be(category);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidCategory(string? category)
    {
        // Act
        var result = ProductCategory.Create(category);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.CategoryRequired);
    }

    [Fact]
    public void Create_ShouldTrimCategory_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string category = "  Power Tools  ";

        // Act
        var result = ProductCategory.Create(category);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Category.Should().Be(category.Trim());
    }
}
