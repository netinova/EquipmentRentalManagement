using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Branch;

public static class BranchErrors
{
    public static readonly Error IdRequired =
        new("Branch.IdRequired", "Id is required.");

    public static readonly Error CityRequired =
        new("Branch.CityRequired", "City is required.");
}