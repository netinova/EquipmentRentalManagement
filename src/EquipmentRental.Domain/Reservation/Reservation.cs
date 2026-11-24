using EquipmentRental.Domain.Abstractions;
using EquipmentRental.Domain.Branch;
using EquipmentRental.Domain.Customer;
using EquipmentRental.Domain.Reservation.Events;

namespace EquipmentRental.Domain.Reservation;

public sealed class Reservation : EventSourcedAggregateRoot, IReconstitutable<Reservation>
{
    private Reservation()
    {
    }

    public ReservationId Id { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public BranchId BranchId { get; private set; } = null!;
    public RentalPeriod RentalPeriod { get; private set; } = null!;
    public ReservationStatus ReservationStatus { get; private set; }

    private readonly List<ReservationLine> _reservationLines = [];

    public IReadOnlyCollection<ReservationLine> ReservationLines =>
        _reservationLines.AsReadOnly();

    public static Result<Reservation> Create(
        CustomerId customerId,
        BranchId branchId,
        RentalPeriod rentalPeriod,
        IReadOnlyCollection<ReservationLine> reservationLines)
    {
        if (!reservationLines.Any())
            return ReservationErrors.ZeroReservationLines;

        var reservation = new Reservation();
        var reservationId = ReservationId.Create(Guid.NewGuid()).Value;

        reservation.RaiseAndApplyEvent(
            new ReservationCreated(reservationId, customerId, branchId, rentalPeriod, reservationLines)
        );

        return reservation;
    }

    public Result Confirm()
    {
        if (ReservationStatus != ReservationStatus.Pending)
            return ReservationErrors.NotPendingStatus;

        RaiseAndApplyEvent(new ReservationConfirmed(Id));

        return Result.Success();
    }

    public Result Cancel()
    {
        if (ReservationStatus == ReservationStatus.Fulfilled)
            return ReservationErrors.CannotCancelFulfilled;

        if (ReservationStatus == ReservationStatus.Cancelled)
            return ReservationErrors.AlreadyCancelled;

        RaiseAndApplyEvent(new ReservationCancelled(Id));

        return Result.Success();
    }

    public Result Fulfill()
    {
        if (ReservationStatus != ReservationStatus.Confirmed)
            return ReservationErrors.NotConfirmedStatus;

        RaiseAndApplyEvent(new ReservationFulfilled(Id));

        return Result.Success();
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case ReservationCreated e:
                Apply(e);
                break;
            case ReservationConfirmed e:
                Apply(e);
                break;
            case ReservationCancelled e:
                Apply(e);
                break;
            case ReservationFulfilled e:
                Apply(e);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported event: {domainEvent.GetType().Name}");
        }
    }

    private void Apply(ReservationCreated @event)
    {
        Id = @event.ReservationId;
        CustomerId = @event.CustomerId;
        BranchId = @event.BranchId;
        RentalPeriod = @event.RentalPeriod;
        ReservationStatus = ReservationStatus.Pending;

        _reservationLines.AddRange(@event.ReservationLines);
    }

    private void Apply(ReservationConfirmed _) => ReservationStatus = ReservationStatus.Confirmed;

    private void Apply(ReservationCancelled _) => ReservationStatus = ReservationStatus.Cancelled;

    private void Apply(ReservationFulfilled _) => ReservationStatus = ReservationStatus.Fulfilled;

    public static Reservation ReplayEvents(IEnumerable<IDomainEvent> history)
    {
        var reservation = new Reservation();

        foreach (var @event in history)
        {
            reservation.Apply(@event);
        }

        return reservation;
    }
}