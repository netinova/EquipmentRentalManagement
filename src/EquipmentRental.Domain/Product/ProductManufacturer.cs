using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Product;

public sealed record ProductManufacturer
{
    public string Manufacturer { get; }

    private ProductManufacturer(string manufacturer) => Manufacturer = manufacturer;

    public static Result<ProductManufacturer> Create(string? manufacturer)
    {
        if (string.IsNullOrWhiteSpace(manufacturer))
            return ProductErrors.ManufacturerRequired;

        return new ProductManufacturer(manufacturer.Trim());
    }
}
