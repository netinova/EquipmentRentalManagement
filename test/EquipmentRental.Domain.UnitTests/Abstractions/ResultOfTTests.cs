using EquipmentRental.Domain.Abstractions;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Abstractions;

public class ResultOfTTests
{
    [Fact]
    public void Success_ShouldContainValue_WhenExpectingSuccessfulResult()
    {
        // Arrange
        int expected = 42;

        // Act
        var result = Result<int>.Success(expected);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(expected);
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Success_ShouldContainError_WhenExpectingFailedResult()
    {
        // Arrange
        var expectedError = new Error("Test.Error", "Something went wrong.");

        // Act
        var result = Result<int>.Failure(expectedError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public void Value_ShouldThrowInvalidOperationException_WhenResultIsFailure()
    {
        // Arrange
        var result = Result<int>.Failure(
            new Error("Test.Error", "Something went wrong."));

        // Act
        var act = () => result.Value;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Success_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        // Act
        var act = () => Result<string>.Success(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        // Act
        var act = () => Result<string>.Failure(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_ShouldThrowArgumentException_WhitNoneError()
    {
        // Arrange
        Error error = Error.None;

        // Act
        var act = () => Result<int>.Failure(error);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}