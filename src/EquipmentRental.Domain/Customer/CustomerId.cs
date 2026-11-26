using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Customer;

public sealed record CustomerId
{
    public Guid Id { get; }

    private CustomerId(Guid id) => Id = id;

    public static Result<CustomerId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return CustomerErrors.IdRequired;

        return new CustomerId(value);
    }
}