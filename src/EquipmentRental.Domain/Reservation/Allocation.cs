using EquipmentRental.Domain.EquipmentUnit;

namespace EquipmentRental.Domain.Reservation;

public sealed class Allocation
{
    public AllocationId AllocationId { get; private set; }
    public EquipmentUnitId EquipmentUnitId { get; private set; }
    public ReservationId ReservationId { get; private set; }
    public RentalPeriod RentalPeriod { get; private set; }
}