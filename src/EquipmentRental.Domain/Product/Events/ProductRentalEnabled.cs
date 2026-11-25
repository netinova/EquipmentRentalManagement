using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Product.Events;

public sealed record ProductRentalEnabled(ProductId Id) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => Id.Id;
}