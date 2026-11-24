using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Reservation.Events;

public sealed record ReservationFulfilled(ReservationId ReservationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => ReservationId.Id;
}