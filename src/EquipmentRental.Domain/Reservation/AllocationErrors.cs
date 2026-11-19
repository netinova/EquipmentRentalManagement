using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Reservation;

public static class AllocationErrors
{
    public static readonly Error IdRequired =
        new("Allocation.IdRequired", "Id is required.");
}