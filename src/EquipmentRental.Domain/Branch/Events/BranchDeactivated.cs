using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Branch.Events;

public sealed record BranchDeactivated(BranchId Id) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => Id.Id;
}