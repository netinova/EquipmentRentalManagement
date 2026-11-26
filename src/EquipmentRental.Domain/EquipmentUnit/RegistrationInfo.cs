using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.EquipmentUnit;

public sealed record RegistrationInfo
{
    public DateOnly AcquiredOn { get; }
    public string Source { get; }

    private RegistrationInfo(DateOnly acquiredOn, string source)
    {
        AcquiredOn = acquiredOn;
        Source = source;
    }

    public static Result<RegistrationInfo> Create(DateOnly acquiredOn, string? source = null)
    {
        return new RegistrationInfo(acquiredOn, source?.Trim() ?? string.Empty);
    }
}
