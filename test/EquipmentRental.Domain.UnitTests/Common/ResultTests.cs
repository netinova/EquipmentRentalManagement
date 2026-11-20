using EquipmentRental.Domain.Common;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldSucceed_WhenExpectingSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Success_ShouldContainError_WhenExpectingFailedResult()
    {
        // Arrange
        var expectedError = new Error("Test.Error", "Something went wrong.");

        // Act
        var result = Result.Failure(expectedError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public void Failure_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        // Act
        var act = () => Result.Failure(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}