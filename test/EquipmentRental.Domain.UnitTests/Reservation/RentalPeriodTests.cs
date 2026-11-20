using EquipmentRental.Domain.Reservation;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Reservation;

public class RentalPeriodTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInputs()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 1, 5);

        // Act
        var result = RentalPeriod.Create(startDate, endDate);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Start.Should().Be(startDate);
        result.Value.End.Should().Be(endDate);
    }

    [Fact]
    public void Create_ShouldFail_WhenStartDateIsNull()
    {
        // Arrange
        DateOnly? startDate = null;
        var endDate = new DateOnly(2026, 1, 5);

        // Act
        var result = RentalPeriod.Create(startDate, endDate);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.StartDateRequired);
    }

    [Fact]
    public void Create_ShouldFail_WhenEndDateIsNull()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 1);
        DateOnly? endDate = null;

        // Act
        var result = RentalPeriod.Create(startDate, endDate);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.EndDateRequired);
    }

    [Fact]
    public void Create_ShouldFail_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 5);
        var endDate = new DateOnly(2026, 1, 1);

        // Act
        var result = RentalPeriod.Create(startDate, endDate);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.EndMustBeAfterStart);
    }

    [Fact]
    public void Create_ShouldFail_WhenEndDateIsStartDate()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 1, 1);

        // Act
        var result = RentalPeriod.Create(startDate, endDate);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.EndMustBeAfterStart);
    }
}