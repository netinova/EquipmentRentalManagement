using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Product;

public sealed record ProductDescription
{
    public string Description { get; }

    private ProductDescription(string description) => Description = description;

    public static Result<ProductDescription> Create(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return ProductErrors.DescriptionRequired;

        if (description.Length > 100)
            return ProductErrors.DescriptionTooLong;

        return new ProductDescription(description.Trim());
    }
};