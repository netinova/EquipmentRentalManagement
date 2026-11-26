using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.EquipmentUnit;

public sealed record SerialNumber
{
    public string Value { get; }

    private SerialNumber(string value) => Value = value.Trim();

    public static Result<SerialNumber> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return EquipmentUnitErrors.SerialNumberRequired;

        return new SerialNumber(value);
    }
}
