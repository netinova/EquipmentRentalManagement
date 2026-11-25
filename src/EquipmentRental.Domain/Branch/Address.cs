using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Branch;

public sealed record Address
{
    public string Street { get; }
    public string City { get; }
    public string Country { get; }

    private Address(string street, string city, string country)
    {
        Street = street;
        City = city;
        Country = country;
    }

    public static Result<Address> Create(string? street, string? city, string? country)
    {
        if (string.IsNullOrWhiteSpace(street))
            return BranchErrors.StreetRequired;

        if (string.IsNullOrWhiteSpace(city))
            return BranchErrors.CityRequired;

        if (string.IsNullOrWhiteSpace(country))
            return BranchErrors.CountryRequired;

        return new Address(street.Trim(), city.Trim(), country.Trim());
    }
}
