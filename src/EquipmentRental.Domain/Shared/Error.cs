namespace EquipmentRental.Domain.Shared;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public bool IsNone => Code == string.Empty;
}