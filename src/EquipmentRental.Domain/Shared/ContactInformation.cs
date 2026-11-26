namespace EquipmentRental.Domain.Shared;

public sealed record ContactInformation
{
    public string Phone { get; }
    public string Email { get; }

    private ContactInformation(string phone, string email)
    {
        Phone = phone;
        Email = email;
    }

    public static ContactInformation Create(string? phone = null, string? email = null)
    {
        return new ContactInformation(phone?.Trim() ?? string.Empty, email?.Trim() ?? string.Empty);
    }
}
