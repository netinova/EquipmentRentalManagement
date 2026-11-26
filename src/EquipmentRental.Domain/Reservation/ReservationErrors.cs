using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Reservation;

public static class ReservationErrors
{
    public static readonly Error IdRequired =
        new("Reservation.IdRequired", "Id is required.");

    public static readonly Error ProductIdRequired =
        new("Reservation.ProductIdRequired", "Product id is required.");

    public static readonly Error QuantityRequired =
        new("Reservation.QuantityRequired", "Quantity is required.");

    public static readonly Error QuantityMustBeMoreThanZero =
        new("Reservation.QuantityMustBeMoreThanZero", "Quantity must be more than 0.");

    public static readonly Error StartDateRequired =
        new("Reservation.StartDateRequired", "Start date is required.");

    public static readonly Error EndDateRequired =
        new("Reservation.EndDateRequired", "End date is required");

    public static readonly Error EndMustBeAfterStart =
        new("Reservation.EndMustBeAfterStart", "End date must be after start date.");

    public static readonly Error ZeroReservationLines =
        new("Reservation.ZeroReservationLines", "Reservation lines should be more than 0.");

    public static readonly Error NotPendingStatus =
        new("Reservation.NotPendingStatus", "Cannot confirm non pending reservation.");

    public static readonly Error CannotCancelFulfilled =
        new("Reservation.CannotCancelFulfilled", "Cannot cancel a fulfilled reservation.");

    public static readonly Error AlreadyCancelled =
        new("Reservation.AlreadyCancelled", "Cannot cancel an already cancelled reservation.");

    public static readonly Error NotConfirmedStatus =
        new("Reservation.NotConfirmedStatus", "Cannot fulfill non confirmed reservation.");
}