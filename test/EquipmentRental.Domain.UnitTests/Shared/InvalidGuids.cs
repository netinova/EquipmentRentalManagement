namespace EquipmentRental.Domain.UnitTests.Shared;

public static class InvalidIds
{
    public static TheoryData<Guid?> Guids =>
    [
        null,
        Guid.Empty
    ];
}