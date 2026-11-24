namespace EquipmentRental.Domain.Abstractions;

public interface IReconstitutable<TSelf> where TSelf : EventSourcedAggregateRoot
{
   static abstract TSelf ReplayEvents(IEnumerable<IDomainEvent> history);
}