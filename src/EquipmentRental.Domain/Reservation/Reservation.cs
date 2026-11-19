using EquipmentRental.Domain.Branch;
using EquipmentRental.Domain.Customer;

namespace EquipmentRental.Domain.Reservation;

public sealed class Reservation
{
    public ReservationId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public BranchId BranchId { get; private set; }
    public RentalPeriod RentalPeriod { get; private set; }
    public ReservationStatus ReservationStatus { get; private set; }
    public ICollection<ReservationLine> ReservationLines {get; private set;}


}