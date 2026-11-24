using EquipmentRental.Domain.Abstractions;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Abstractions;

public class ErrorTests
{
    [Fact]
    public void IsNone_ShouldReturnTrue_OnNoneError()
    {
        // Act
        var result = Error.None.IsNone;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNone_ShouldReturnTrue_OnRegularError()
    {
        // Arrange
        Error error = new("Test.test", "Test error.");

        // Act
        var result = error.IsNone;

        // Assert
        result.Should().BeFalse();
    }
}