using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Branch.Events;

namespace EquipmentRental.Domain.Branch;

public sealed class Branch : EventSourcedAggregateRoot, IReconstitutable<Branch>
{
    private Branch()
    {
    }

    public BranchId Id { get; private set; } = null!;
    public BranchName Name { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public ContactInformation ContactInformation { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public static Result<Branch> Create(BranchName name, Address address, ContactInformation contactInformation)
    {
        var branch = new Branch();

        var id = BranchId.Create(Guid.NewGuid()).Value;

        branch.RaiseAndApplyEvent(
            new BranchCreated(id, name, address, contactInformation)
        );

        return branch;
    }

    public Result Activate()
    {
        RaiseAndApplyEvent(new BranchActivated(Id));

        return Result.Success();
    }

    public Result Deactivate()
    {
        RaiseAndApplyEvent(new BranchDeactivated(Id));

        return Result.Success();
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case BranchCreated e:
                Apply(e);
                break;
            case BranchActivated e:
                Apply(e);
                break;
            case BranchDeactivated e:
                Apply(e);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported event: {domainEvent.GetType().Name}");
        }
    }

    private void Apply(BranchCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Address = @event.Address;
        ContactInformation = @event.ContactInformation;
        IsActive = true;
    }

    private void Apply(BranchActivated _)
    {
        IsActive = true;
    }

    private void Apply(BranchDeactivated _)
    {
        IsActive = false;
    }

    public static Branch ReplayEvents(IEnumerable<IDomainEvent> history)
    {
        var branch = new Branch();

        foreach (var @event in history)
        {
            branch.Apply(@event);
        }

        return branch;
    }
}
