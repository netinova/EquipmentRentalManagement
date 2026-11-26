using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.EquipmentUnit;
using EquipmentRental.Domain.Product.Events;
using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Product;

public sealed class Product : EventSourcedAggregateRoot, IReconstitutable<Product>
{
    private Product()
    {
    }

    public ProductId Id { get; private set; } = null!;
    public ProductName Name { get; private set; } = null!;
    public ProductDescription Description { get; private set; } = null!;
    public ProductManufacturer Manufacturer { get; private set; } = null!;
    public ProductModel Model { get; private set; } = null!;
    public RentAvailability RentAvailability { get; private set; } = null!;
    public RentalRate RentalRate { get; private set; } = null!;
    public HandlingNotes HandlingNotes { get; private set; } = null!;

    private readonly List<EquipmentUnitId> _equipmentUnits = [];

    public IReadOnlyCollection<EquipmentUnitId> EquipmentUnits => _equipmentUnits.AsReadOnly();

    public static Result<Product> Create(ProductName name, ProductDescription description,
        ProductManufacturer manufacturer, ProductModel model, RentalRate rentalRate,
        IReadOnlyCollection<EquipmentUnitId>? equipmentUnits = null,
        HandlingNotes? handlingNotes = null)
    {
        var product = new Product();

        var id = ProductId.Create(Guid.NewGuid()).Value;

        product.RaiseAndApplyEvent(new ProductCreated(id, name, description, manufacturer, model,
            rentalRate,
            equipmentUnits ?? [],
            handlingNotes ?? HandlingNotes.Create(string.Empty).Value));

        return product;
    }

    public Result EnableRental()
    {
        RaiseAndApplyEvent(new ProductRentalEnabled(Id));

        return Result.Success();
    }

    public Result DisableRental()
    {
        RaiseAndApplyEvent(new ProductRentalDisabled(Id));

        return Result.Success();
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case ProductCreated e:
                Apply(e);
                break;
            case ProductRentalEnabled e:
                Apply(e);
                break;
            case ProductRentalDisabled e:
                Apply(e);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported event: {domainEvent.GetType().Name}");
        }
    }

    private void Apply(ProductCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Description = @event.Description;
        Manufacturer = @event.Manufacturer;
        Model = @event.Model;
        RentAvailability = RentAvailability.ForRental();
        RentalRate = @event.RentalRate;
        HandlingNotes = @event.HandlingNotes;

        _equipmentUnits.AddRange(@event.EquipmentUnits);
    }

    private void Apply(ProductRentalEnabled _) => RentAvailability = RentAvailability.ForRental();
    private void Apply(ProductRentalDisabled _) => RentAvailability = RentAvailability.NotForRental();

    public static Product ReplayEvents(IEnumerable<IDomainEvent> history)
    {
        var product = new Product();

        foreach (var @event in history)
        {
            product.Apply(@event);
        }

        return product;
    }
}