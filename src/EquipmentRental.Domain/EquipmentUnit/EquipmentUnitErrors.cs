using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.EquipmentUnit;

public static class EquipmentUnitErrors
{
    public static readonly Error IdRequired =
        new("EquipmentUnit.IdRequired", "Id is required.");
}