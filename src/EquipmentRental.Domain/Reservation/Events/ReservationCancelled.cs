using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Reservation.Events;

public sealed record ReservationCancelled(ReservationId ReservationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => ReservationId.Id;
}