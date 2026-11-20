using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Allocation;

public sealed record AllocationId
{
    public Guid Id { get; }

    private AllocationId(Guid id) => Id = id;

    public static Result<AllocationId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return AllocationErrors.IdRequired;

        return new AllocationId(value);
    }
}