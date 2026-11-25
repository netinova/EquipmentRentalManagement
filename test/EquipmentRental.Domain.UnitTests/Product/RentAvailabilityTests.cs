using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class RentAvailabilityTests
{
    [Fact]
    public void ForRental_ShouldSetIsRentableToTrue()
    {
        // Act
        var availability = RentAvailability.ForRental();

        // Assert
        availability.IsRentable.Should().BeTrue();
    }

    [Fact]
    public void NotForRental_ShouldSetIsRentableToFalse()
    {
        // Act
        var availability = RentAvailability.NotForRental();

        // Assert
        availability.IsRentable.Should().BeFalse();
    }
}
