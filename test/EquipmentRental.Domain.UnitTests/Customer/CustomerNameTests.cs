using EquipmentRental.Domain.Customer;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Customer;

public class CustomerNameTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        string firstName = "john";
        string lastName = "smith";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithInvalidFirstName(string? firstName)
    {
        // Arrange
        string lastName = "smith";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.FirstNameRequired);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldFail_WithNullLastName(string? lastName)
    {
        // Arrange
        string firstName = "john";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CustomerErrors.LastNameRequired);
    }

    [Fact]
    public void Create_ShouldTrimNames_WithSurroundingWhiteSpaces()
    {
        // Arrange
        string firstName = "  john  ";
        string lastName = "  smith  ";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be(firstName.Trim());
        result.Value.LastName.Should().Be(lastName.Trim());
    }
}