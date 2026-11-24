using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Branch;
using EquipmentRental.Domain.Customer;
using EquipmentRental.Domain.Product;
using EquipmentRental.Domain.Reservation;
using EquipmentRental.Domain.Reservation.Events;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Reservation;

public class ReservationTests
{
    private readonly CustomerId _customerId = CustomerId.Create(Guid.NewGuid()).Value;
    private readonly BranchId _branchId = BranchId.Create(Guid.NewGuid()).Value;

    private readonly RentalPeriod _rentalPeriod = RentalPeriod.Create(
        new DateOnly(2026, 1, 1),
        new DateOnly(2026, 1, 5)
    ).Value;

    private readonly IReadOnlyCollection<ReservationLine> _reservationLines = new List<ReservationLine>()
    {
        ReservationLine.Create(ProductId.Create(Guid.NewGuid()).Value, 1u).Value,
        ReservationLine.Create(ProductId.Create(Guid.NewGuid()).Value, 2u).Value,
    }.AsReadOnly();

    [Fact]
    public void Create_ShouldSucceedAndRaiseEvent_WhitValidInput()
    {
        // Act
        var result = Domain.Reservation.Reservation.Create(_customerId, _branchId, _rentalPeriod, _reservationLines);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var reservation = result.Value;

        // Assert State Properties
        reservation.CustomerId.Should().Be(_customerId);
        reservation.BranchId.Should().Be(_branchId);
        reservation.RentalPeriod.Should().Be(_rentalPeriod);
        reservation.ReservationLines.Should().BeEquivalentTo(_reservationLines);

        // Assert Domain Events
        reservation.DomainEvents.Should().ContainSingle();

        var createdEvent = reservation.DomainEvents.First() as ReservationCreated;

        createdEvent.Should().NotBeNull();
        createdEvent.ReservationId.Should().Be(reservation.Id);
        createdEvent.CustomerId.Should().Be(_customerId);
        createdEvent.BranchId.Should().Be(_branchId);
        createdEvent.RentalPeriod.Should().Be(_rentalPeriod);
        createdEvent.ReservationLines.Should().BeEquivalentTo(_reservationLines);
    }

    [Fact]
    public void Create_ShouldFail_WhenThereIsZeroReservationLines()
    {
        // Act
        var result =
            Domain.Reservation.Reservation.Create(_customerId, _branchId, _rentalPeriod, []);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.ZeroReservationLines);
    }

    [Fact]
    public void Confirm_ShouldSuccessAndRaiseAndApplyEvent_WhenStatusIsPending()
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        reservation.ClearDomainEvents();

        // Act
        var result = reservation.Confirm();

        // Assert
        result.IsSuccess.Should().BeTrue();
        reservation.ReservationStatus.Should().Be(ReservationStatus.Confirmed);

        // Assert Domain Events
        reservation.DomainEvents.Should().ContainSingle();

        var createdEvent = reservation.DomainEvents.Last() as ReservationConfirmed;
        createdEvent.Should().NotBeNull();
        createdEvent.ReservationId.Should().Be(reservation.Id);
    }

    [Theory]
    [InlineData(ReservationStatus.Confirmed)]
    [InlineData(ReservationStatus.Fulfilled)]
    [InlineData(ReservationStatus.Cancelled)]
    public void Confirm_ShouldFail_WhenStatusIsNotPending(ReservationStatus invalidStatus)
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        switch (invalidStatus)
        {
            case ReservationStatus.Confirmed:
                reservation.Confirm();
                break;
            case ReservationStatus.Fulfilled:
                reservation.Confirm();
                reservation.Fulfill();
                break;
            case ReservationStatus.Cancelled:
                reservation.Cancel();
                break;
        }

        reservation.ReservationStatus.Should().Be(invalidStatus);

        // Act
        var result = reservation.Confirm();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.NotPendingStatus);
    }

    [Theory]
    [InlineData(ReservationStatus.Pending)]
    [InlineData(ReservationStatus.Confirmed)]
    public void Cancel_ShouldSucceedAndRaiseAndApplyEvent_WhenStatusIsNotCancelledOrFulFilled(
        ReservationStatus validStatus)
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        switch (validStatus)
        {
            case ReservationStatus.Confirmed:
                reservation.Confirm();
                break;
            case ReservationStatus.Pending:
                break;
        }

        reservation.ReservationStatus.Should().Be(validStatus);
        reservation.ClearDomainEvents();

        // Act
        var result = reservation.Cancel();

        // Assert
        result.IsSuccess.Should().BeTrue();
        reservation.ReservationStatus.Should().Be(ReservationStatus.Cancelled);


        // Assert Domain Events
        reservation.DomainEvents.Should().ContainSingle();

        var createdEvent = reservation.DomainEvents.Last() as ReservationCancelled;
        createdEvent.Should().NotBeNull();
        createdEvent.ReservationId.Should().Be(reservation.Id);
    }

    [Fact]
    public void Cancel_ShouldFail_WhenStatusIsCancelled()
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        reservation.Cancel();

        // Act
        var result = reservation.Cancel();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.AlreadyCancelled);
    }

    [Fact]
    public void Cancel_ShouldFail_WhenStatusIsFullFilled()
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        reservation.Confirm();
        reservation.Fulfill();

        // Act
        var result = reservation.Cancel();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.CannotCancelFulfilled);
    }

    [Fact]
    public void Fulfill_ShouldSucceed_WhenStatusIsConfirmed()
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        reservation.Confirm();
        reservation.ClearDomainEvents();

        // Act
        var result = reservation.Fulfill();

        // Assert
        result.IsSuccess.Should().BeTrue();
        reservation.ReservationStatus.Should().Be(ReservationStatus.Fulfilled);

        // Assert Domain Events
        reservation.DomainEvents.Should().ContainSingle();

        var createdEvent = reservation.DomainEvents.Last() as ReservationFulfilled;
        createdEvent.Should().NotBeNull();
        createdEvent.ReservationId.Should().Be(reservation.Id);
    }

    [Theory]
    [InlineData(ReservationStatus.Pending)]
    [InlineData(ReservationStatus.Cancelled)]
    [InlineData(ReservationStatus.Fulfilled)]
    public void Fulfill_ShouldFail_WhenStatusIsNotConfirmed(ReservationStatus invalidStatus)
    {
        // Arrange
        var reservation = Domain.Reservation.Reservation
            .Create(_customerId, _branchId, _rentalPeriod, _reservationLines)
            .Value;

        switch (invalidStatus)
        {
            case ReservationStatus.Pending:
                break;
            case ReservationStatus.Fulfilled:
                reservation.Confirm();
                reservation.Fulfill();
                break;
            case ReservationStatus.Cancelled:
                reservation.Cancel();
                break;
        }

        reservation.ReservationStatus.Should().Be(invalidStatus);

        // Act
        var result = reservation.Fulfill();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.NotConfirmedStatus);
    }

    [Theory]
    [InlineData(ReservationStatus.Pending)]
    [InlineData(ReservationStatus.Confirmed)]
    [InlineData(ReservationStatus.Fulfilled)]
    [InlineData(ReservationStatus.Cancelled)]
    public void ReplayEvents_ShouldReconstituteCorrectState(ReservationStatus targetStatus)
    {
        // Arrange
        var reservationId = ReservationId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ReservationCreated(reservationId, _customerId, _branchId, _rentalPeriod, _reservationLines)
        };

        switch (targetStatus)
        {
            case ReservationStatus.Confirmed:
                history.Add(new ReservationConfirmed(reservationId));
                break;
            case ReservationStatus.Fulfilled:
                history.Add(new ReservationConfirmed(reservationId));
                history.Add(new ReservationFulfilled(reservationId));
                break;
            case ReservationStatus.Cancelled:
                history.Add(new ReservationCancelled(reservationId));
                break;
        }

        // Act
        var reservation = Domain.Reservation.Reservation.ReplayEvents(history);

        // Assert
        reservation.Id.Should().Be(reservationId);
        reservation.CustomerId.Should().Be(_customerId);
        reservation.BranchId.Should().Be(_branchId);
        reservation.RentalPeriod.Should().Be(_rentalPeriod);
        reservation.ReservationLines.Should().BeEqualTo(_reservationLines);
        reservation.ReservationStatus.Should().Be(targetStatus);
    }

    [Fact]
    public void ReplayEvents_ShouldNotRaiseNewDomainEvents()
    {
        // Arrange
        var reservationId = ReservationId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new ReservationCreated(reservationId, _customerId, _branchId, _rentalPeriod, _reservationLines),
            new ReservationConfirmed(reservationId)
        };

        // Act
        var reservation = Domain.Reservation.Reservation.ReplayEvents(history);

        // Assert
        reservation.DomainEvents.Should().BeEmpty();
    }

    public record UnsupportedEvent(ReservationId ReservationId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid StreamId => ReservationId.Id;
    }

    [Fact]
    public void ReplayEvents_ShouldThrowException_WhenUnsupportedEventIsEncountered()
    {
        // Arrange
        var reservationId = ReservationId.Create(Guid.NewGuid()).Value;
        var history = new List<IDomainEvent>
        {
            new UnsupportedEvent(reservationId)
        };

        // Act
        var act = () => Domain.Reservation.Reservation.ReplayEvents(history);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Unsupported event: {nameof(UnsupportedEvent)}");
    }
}