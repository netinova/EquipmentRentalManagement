using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Allocation;

public static class AllocationErrors
{
    public static readonly Error IdRequired =
        new("Allocation.IdRequired", "Id is required.");
}