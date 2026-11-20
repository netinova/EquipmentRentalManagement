using EquipmentRental.Domain.EquipmentUnit;
using EquipmentRental.Domain.UnitTests.Shared;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.EquipmentUnit;

public class EquipmentUnitIdTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        var result = EquipmentUnitId.Create(guid);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(guid);
    }

    [Theory]
    [MemberData(nameof(InvalidIds.Guids), MemberType = typeof(InvalidIds))]
    public void Create_ShouldFail_WithInvalidGuid(Guid? guid)
    {
        // Act
        var result = EquipmentUnitId.Create(guid);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(EquipmentUnitErrors.IdRequired);
    }
}