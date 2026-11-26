using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Product;

public sealed record HandlingNotes
{
    public string Notes { get; }

    private HandlingNotes(string notes) => Notes = notes;

    public static Result<HandlingNotes> Create(string? notes)
    {
        if (notes is null)
            return ProductErrors.HandlingNotesRequired;

        return new HandlingNotes(notes.Trim());
    }
}
