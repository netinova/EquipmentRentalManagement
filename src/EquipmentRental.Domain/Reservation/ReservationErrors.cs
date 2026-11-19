using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Reservation;

public static class ReservationErrors
{
    public static readonly Error IdRequired =
        new("Reservation.IdRequired", "Id is required.");

    public static readonly Error QuantityMustBeMoreThanZero =
        new("Reservation.QuantityMustBeMoreThanZero", "Quantity must be more than 0.");

    public static readonly Error EndMustBeAfterStart =
        new("Reservation.EndMustBeAfterStart", "End date must be after start date.");

}