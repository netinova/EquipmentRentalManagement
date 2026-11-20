using EquipmentRental.Domain.Common;
using EquipmentRental.Domain.Product;

namespace EquipmentRental.Domain.Reservation;

public sealed record ReservationLine
{
    public ProductId ProductId { get; }
    public uint Quantity { get; }

    private ReservationLine(ProductId productId, uint quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public static Result<ReservationLine> Create(ProductId? productId, uint? quantity)
    {
        if (productId is not { } productIdValue)
            return ReservationErrors.ProductIdRequired;

        if (quantity is not { } quantityValue)
            return ReservationErrors.QuantityRequired;

        if (quantityValue < 1)
            return ReservationErrors.QuantityMustBeMoreThanZero;

        return new ReservationLine(productIdValue, quantityValue);
    }
}