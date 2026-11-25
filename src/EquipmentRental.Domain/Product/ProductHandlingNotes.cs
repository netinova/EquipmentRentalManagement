using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Product;

public sealed record ProductHandlingNotes
{
    public string? Notes { get; }

    private ProductHandlingNotes(string? notes) => Notes = notes;

    public static Result<ProductHandlingNotes> Create(string? notes)
        => new ProductHandlingNotes(notes?.Trim());
}
