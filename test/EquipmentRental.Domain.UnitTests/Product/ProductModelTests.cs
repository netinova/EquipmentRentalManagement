using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductModelTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        const string model = "DCD791";

        // Act
        var result = ProductModel.Create(model);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Model.Should().Be(model);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidModel(string? model)
    {
        // Act
        var result = ProductModel.Create(model);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ModelRequired);
    }

    [Fact]
    public void Create_ShouldTrimModel_WithSurroundingWhiteSpaces()
    {
        // Arrange
        const string model = "  DCD791  ";

        // Act
        var result = ProductModel.Create(model);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Model.Should().Be(model.Trim());
    }
}
