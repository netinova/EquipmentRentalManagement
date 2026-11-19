using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Customer;

public sealed record CustomerId
{
    public Guid Id { get; }

    private CustomerId(Guid id) => Id = id;

    public static Result<CustomerId> Create(Guid id)
    {
        if (id == Guid.Empty)
            return CustomerErrors.IdRequired;

        return new CustomerId(id);
    }
}