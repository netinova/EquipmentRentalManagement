using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Reservation;

public sealed record ReservationId
{
    public Guid Id { get; }

    private ReservationId(Guid id) => Id = id;

    public static Result<ReservationId> Create(Guid? id)
    {
        if (id is not { } value || value == Guid.Empty)
            return ReservationErrors.IdRequired;

        return new ReservationId(value);
    }
}