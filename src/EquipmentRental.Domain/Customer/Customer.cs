
using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Customer;

public sealed class Customer
{
    public CustomerId Id { get; private set; } = null!;
    public CustomerName Name { get; private set; } = null!;
    public ContactInformation ContactInformation { get; private set; } = null!;
    public DateTime RegisteredOn { get; private set; } = DateTime.UtcNow;

}

