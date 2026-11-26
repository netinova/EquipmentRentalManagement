using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Product;
using EquipmentRental.Domain.Product.Events;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class ProductDomainTests
{
    private readonly ProductName _productName = ProductName.Create("Camera").Value;
    private readonly ProductDescription _productDescription = ProductDescription.Create("A high-quality camera.").Value;
    private readonly ProductManufacturer _manufacturer = ProductManufacturer.Create("Sony").Value;
    private readonly ProductModel _model = ProductModel.Create("DCD791").Value;
    private readonly RentalRate _rentalRate = RentalRate.Create(25.00m, PricingBasis.Daily).Value;

    [Fact]
    public void Create_ShouldSucceedAndRaiseEvent_WhenValidInput()
    {
        // Act
        var result = Domain.Product.Product.Create(
            _productName, _productDescription, _manufacturer, _model, _rentalRate);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var product = result.Value;

        product.Id.Should().NotBe(Guid.Empty);
        product.Name.Should().Be(_productName);
        product.Description.Should().Be(_productDescription);
        product.Manufacturer.Should().Be(_manufacturer);
        product.Model.Should().Be(_model);
        product.RentalRate.Should().Be(_rentalRate);
        product.RentAvailability.IsRentable.Should().BeTrue();

        // Assert Domain Events
        product.DomainEvents.Should().ContainSingle();

        var createdEvent = product.DomainEvents.First() as ProductCreated;
        createdEvent.Should().NotBeNull();
        createdEvent.Id.Should().Be(product.Id);
        createdEvent.Name.Should().Be(_productName);
        createdEvent.Description.Should().Be(_productDescription);
        createdEvent.Manufacturer.Should().Be(_manufacturer);
        createdEvent.Model.Should().Be(_model);
        createdEvent.RentalRate.Should().Be(_rentalRate);
    }

    [Fact]
    public void EnableRental_ShouldSucceedAndRaiseEvent()
    {
        // Arrange
        var product = Domain.Product.Product.Create(
                _productName, _productDescription, _manufacturer, _model, _rentalRate)
            .Value;

        product.DisableRental();
        product.ClearDomainEvents();

        // Act
        var result = product.EnableRental();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.RentAvailability.IsRentable.Should().BeTrue();

        // Assert Domain Events
        product.DomainEvents.Should().ContainSingle();

        var enabledEvent = product.DomainEvents.First() as ProductRentalEnabled;
        enabledEvent.Should().NotBeNull();
        enabledEvent.Id.Should().Be(product.Id);
    }

    [Fact]
    public void DisableRental_ShouldSucceedAndRaiseEvent()
    {
        // Arrange
        var product = Domain.Product.Product.Create(
                _productName, _productDescription, _manufacturer, _model, _rentalRate)
            .Value;

        product.ClearDomainEvents();

        // Act
        var result = product.DisableRental();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.RentAvailability.IsRentable.Should().BeFalse();

        // Assert Domain Events
        product.DomainEvents.Should().ContainSingle();

        var disabledEvent = product.DomainEvents.First() as ProductRentalDisabled;
        disabledEvent.Should().NotBeNull();
        disabledEvent.Id.Should().Be(product.Id);
    }

    [Fact]
    public void EnableRental_ShouldSucceed_WhenAlreadyEnabled()
    {
        // Arrange
        var product = Domain.Product.Product.Create(
                _productName, _productDescription, _manufacturer, _model, _rentalRate)
            .Value;

        product.ClearDomainEvents();

        // Act
        var result = product.EnableRental();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.RentAvailability.IsRentable.Should().BeTrue();
    }

    [Fact]
    public void DisableRental_ShouldSucceed_WhenAlreadyDisabled()
    {
        // Arrange
        var product = Domain.Product.Product.Create(
                _productName, _productDescription, _manufacturer, _model, _rentalRate)
            .Value;

        product.DisableRental();
        product.ClearDomainEvents();

        // Act
        var result = product.DisableRental();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.RentAvailability.IsRentable.Should().BeFalse();
    }

    [Fact]
    public void ReplayEvents_ShouldReconstituteCorrectState()
    {
        // Arrange
        var id = ProductId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ProductCreated(
                id, _productName, _productDescription, _manufacturer, _model, _rentalRate, [],
                HandlingNotes.Create(string.Empty).Value)
        };

        // Act
        var product = Domain.Product.Product.ReplayEvents(history);

        // Assert
        product.Id.Should().Be(id);
        product.Name.Should().Be(_productName);
        product.Description.Should().Be(_productDescription);
        product.Manufacturer.Should().Be(_manufacturer);
        product.Model.Should().Be(_model);
        product.RentalRate.Should().Be(_rentalRate);
        product.RentAvailability.IsRentable.Should().BeTrue();
    }

    [Fact]
    public void ReplayEvents_ShouldReconstituteCorrectState_WithDisabledState()
    {
        // Arrange
        var id = ProductId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ProductCreated(
                id, _productName, _productDescription, _manufacturer, _model, _rentalRate, [],
                HandlingNotes.Create(string.Empty).Value),
            new ProductRentalDisabled(id)
        };

        // Act
        var product = Domain.Product.Product.ReplayEvents(history);

        // Assert
        product.RentAvailability.IsRentable.Should().BeFalse();
    }

    [Fact]
    public void ReplayEvents_ShouldReconstituteCorrectState_WithReEnabledState()
    {
        // Arrange
        var id = ProductId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ProductCreated(
                id, _productName, _productDescription, _manufacturer, _model, _rentalRate, [],
                HandlingNotes.Create(string.Empty).Value),
            new ProductRentalDisabled(id),
            new ProductRentalEnabled(id)
        };

        // Act
        var product = Domain.Product.Product.ReplayEvents(history);

        // Assert
        product.RentAvailability.IsRentable.Should().BeTrue();
    }

    [Fact]
    public void ReplayEvents_ShouldNotRaiseNewDomainEvents()
    {
        // Arrange
        var id = ProductId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ProductCreated(
                id, _productName, _productDescription, _manufacturer, _model, _rentalRate, [],
                HandlingNotes.Create(string.Empty).Value),
            new ProductRentalDisabled(id)
        };

        // Act
        var product = Domain.Product.Product.ReplayEvents(history);

        // Assert
        product.DomainEvents.Should().BeEmpty();
    }

    public record UnsupportedEvent(ProductId ProductId) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
        public Guid StreamId => ProductId.Id;
    }

    [Fact]
    public void ReplayEvents_ShouldThrowException_WhenUnsupportedEventIsEncountered()
    {
        // Arrange
        var id = ProductId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new UnsupportedEvent(id)
        };

        // Act
        var act = () => Domain.Product.Product.ReplayEvents(history);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Unsupported event: {nameof(UnsupportedEvent)}");
    }
}
