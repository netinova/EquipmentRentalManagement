using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.EquipmentUnit;

public sealed record EquipmentUnitId
{
    public Guid Id { get; }

    private EquipmentUnitId(Guid id) => Id = id;

    public static Result<EquipmentUnitId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return EquipmentUnitErrors.IdRequired;

        return new EquipmentUnitId(value);
    }
}