using EquipmentRental.Domain.EquipmentUnit;
using EquipmentRental.Domain.Reservation;

namespace EquipmentRental.Domain.Allocation;

internal sealed class Allocation
{
    public AllocationId AllocationId { get; private set; }
    public EquipmentUnitId EquipmentUnitId { get; private set; }
    public ReservationId ReservationId { get; private set; }
    public RentalPeriod RentalPeriod { get; private set; }
}