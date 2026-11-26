using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Branch;

public sealed record BranchId
{
    public Guid Id { get; }

    private BranchId(Guid id) => Id = id;

    public static Result<BranchId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return BranchErrors.IdRequired;

        return new BranchId(value);
    }
};