using EquipmentRental.Domain.Branch;
using EquipmentRental.Domain.Product;

namespace EquipmentRental.Domain.EquipmentUnit;

public sealed class EquipmentUnit
{
    public EquipmentUnitId Id { get; private set; }
    public ProductId ProductId { get; private set; }
    public BranchId BranchId { get; private set; }
    public OperationalStatus OperationalStatus { get; private set; }
}