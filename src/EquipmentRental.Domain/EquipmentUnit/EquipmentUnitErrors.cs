using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.EquipmentUnit;

public static class EquipmentUnitErrors
{
    public static readonly Error IdRequired =
        new("EquipmentUnit.IdRequired", "Id is required.");

    public static readonly Error SerialNumberRequired =
        new("EquipmentUnit.SerialNumberRequired", "Serial number is required.");
}