using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Branch;
using EquipmentRental.Domain.Customer;

namespace EquipmentRental.Domain.Reservation.Events;

public sealed record ReservationCreated(
    ReservationId ReservationId,
    CustomerId CustomerId,
    BranchId BranchId,
    RentalPeriod RentalPeriod,
    IReadOnlyCollection<ReservationLine> ReservationLines) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => ReservationId.Id;
}