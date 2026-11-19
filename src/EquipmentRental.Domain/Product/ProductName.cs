using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Product;

public sealed record ProductName
{
    public string Name { get; init; }

    private ProductName(string name) => Name = name;

    public static Result<ProductName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;

        name = name.Trim();

        return new ProductName(name);
    }
}