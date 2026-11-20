using EquipmentRental.Domain.Branch;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Branch;

public class BranchLocationTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string city = "Qazvin";

        // Act
        var result = BranchLocation.Create(city);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.City.Should().Be(city);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_ShouldFail_WithInvalidInput(string? city)
    {
        // Act
        var result = BranchLocation.Create(city);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BranchErrors.CityRequired);
    }

    [Fact]
    public void Create_ShouldTrimCityName_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string city = "  Qazvin  ";

        // Act
        var result = BranchLocation.Create(city);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.City.Should().Be(city.Trim());
    }
}