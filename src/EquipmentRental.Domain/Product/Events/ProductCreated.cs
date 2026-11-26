using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.EquipmentUnit;

namespace EquipmentRental.Domain.Product.Events;

public sealed record ProductCreated(
    ProductId Id,
    ProductName Name,
    ProductDescription Description,
    ProductManufacturer Manufacturer,
    ProductModel Model,
    RentalRate RentalRate,
    IReadOnlyCollection<EquipmentUnitId> EquipmentUnits,
    HandlingNotes HandlingNotes) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid StreamId => Id.Id;
}
