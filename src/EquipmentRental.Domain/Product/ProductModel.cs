using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Product;

public sealed record ProductModel
{
    public string Model { get; }

    private ProductModel(string model) => Model = model;

    public static Result<ProductModel> Create(string? model)
    {
        if (string.IsNullOrWhiteSpace(model))
            return ProductErrors.ModelRequired;

        return new ProductModel(model.Trim());
    }
}
