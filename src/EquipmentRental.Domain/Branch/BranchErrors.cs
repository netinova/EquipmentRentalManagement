using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Branch;

public static class BranchErrors
{
    public static readonly Error IdRequired =
        new("Branch.IdRequired", "Id is required.");

    public static readonly Error NameRequired =
        new("Branch.NameRequired", "Name is required.");

    public static readonly Error CityRequired =
        new("Branch.CityRequired", "City is required.");

    public static readonly Error StreetRequired =
        new("Branch.StreetRequired", "Street address is required.");

    public static readonly Error CountryRequired =
        new("Branch.CountryRequired", "Country is required.");
}
