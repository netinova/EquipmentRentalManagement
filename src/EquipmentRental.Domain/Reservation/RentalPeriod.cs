using EquipmentRental.Domain.Common;

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

    public static Result<RentalPeriod> Create(DateOnly start, DateOnly end)
    {
        if (end <= start)
            return ReservationErrors.EndMustBeAfterStart;

        return new RentalPeriod(start, end);
    }
}