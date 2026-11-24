namespace EquipmentRental.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid StreamId { get; }
}