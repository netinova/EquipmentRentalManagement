using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Branch.Events;

public sealed record BranchCreated(BranchId Id, BranchName Name, Address Address, ContactInformation ContactInformation)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => Id.Id;
}