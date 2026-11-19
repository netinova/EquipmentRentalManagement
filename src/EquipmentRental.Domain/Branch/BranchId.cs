using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Branch;

public record BranchId
{
    public Guid Id { get; init; }

    private BranchId(Guid id) => Id = id;

    public static Result<BranchId> Create(Guid id)
    {
        if (id == Guid.Empty)
            return BranchErrors.IdRequired;

        return new BranchId(id);
    }
};