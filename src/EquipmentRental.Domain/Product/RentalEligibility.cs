namespace EquipmentRental.Domain.Product;

public sealed record RentAvailability
{
    public bool IsRentable { get; }

    private RentAvailability(bool isRentable) => IsRentable = isRentable;

    public static RentAvailability ForRental() => new(true);

    public static RentAvailability NotForRental() => new(false);
}
