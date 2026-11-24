using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Allocation;

public static class AllocationErrors
{
    public static readonly Error IdRequired =
        new("Allocation.IdRequired", "Id is required.");
}