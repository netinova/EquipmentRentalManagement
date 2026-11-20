using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductNameTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string name = "abc";

        // Act
        var result = ProductName.Create(name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidFirstName(string? name)
    {
        // Act
        var result = ProductName.Create(name);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.NameRequired);
    }

    [Fact]
    public void Create_ShouldTrimName_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string name = "  abc  ";

        // Act
        var result = ProductName.Create(name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name.Trim());
    }
}