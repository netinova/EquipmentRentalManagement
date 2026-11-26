using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Product;

public sealed record ProductCategory
{
    public string Category { get; }

    private ProductCategory(string category) => Category = category;

    public static Result<ProductCategory> Create(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return ProductErrors.CategoryRequired;

        return new ProductCategory(category.Trim());
    }
}
