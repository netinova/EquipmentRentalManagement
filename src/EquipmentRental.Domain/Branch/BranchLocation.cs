using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Branch;

public record BranchLocation
{
    public string City { get; }

    private BranchLocation(string city) => City = city;

    public static Result<BranchLocation> Create(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BranchErrors.CityRequired;

        city = city.Trim();

        return new BranchLocation(city);
    }
}