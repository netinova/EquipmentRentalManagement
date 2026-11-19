namespace EquipmentRental.Domain.Branch;

public sealed class Branch
{
    public BranchId Id { get; private set; }
    public BranchLocation Location { get; private set; }
}