using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Reservation;

public sealed record RentalPeriod
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    private RentalPeriod(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public static Result<RentalPeriod> Create(DateOnly? start, DateOnly? end)
    {
        if (start is not { } startValue)
            return ReservationErrors.StartDateRequired;

        if (end is not { } endValue)
            return ReservationErrors.EndDateRequired;

        if (endValue <= startValue)
            return ReservationErrors.EndMustBeAfterStart;

        return new RentalPeriod(startValue, endValue);
    }
}