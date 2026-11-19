using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.EquipmentUnit;

public sealed record EquipmentUnitId
{
    public Guid Id { get; }

    private EquipmentUnitId(Guid id) => Id = id;

    public static Result<EquipmentUnitId> Create(Guid id)
    {
        if (id == Guid.Empty)
            return EquipmentUnitErrors.IdRequired;

        return new EquipmentUnitId(id);
    }
}