using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Branch;

public sealed record BranchName
{
    public string Name { get; }

    private BranchName(string name) => Name = name;

    public static Result<BranchName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BranchErrors.NameRequired;

        return new BranchName(name.Trim());
    }
}