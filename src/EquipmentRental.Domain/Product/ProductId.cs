using EquipmentRental.Domain.Abstractions;


namespace EquipmentRental.Domain.Product;

public sealed record ProductId
{
    public Guid Id { get; }

    private ProductId(Guid id) => Id = id;

    public static Result<ProductId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return ProductErrors.IdRequired;

        return new ProductId(value);
    }
}
