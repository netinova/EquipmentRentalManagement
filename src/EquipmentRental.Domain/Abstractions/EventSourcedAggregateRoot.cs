namespace EquipmentRental.Domain.Abstractions;

public abstract class EventSourcedAggregateRoot : AggregateRoot
{
    protected void RaiseAndApplyEvent(IDomainEvent domainEvent)
    {
        Apply(domainEvent);
        RaiseDomainEvent(domainEvent);
    }

    protected abstract void Apply(IDomainEvent domainEvent);
}