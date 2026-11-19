using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Reservation;

public sealed record AllocationId
{
    public Guid Id { get; }

    private AllocationId(Guid id) => Id = id;

    public static Result<AllocationId> Create(Guid id)
    {
        if (id == Guid.Empty)
            return AllocationErrors.IdRequired;

        return new AllocationId(id);
    }
}