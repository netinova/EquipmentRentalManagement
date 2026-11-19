namespace EquipmentRental.Domain.Customer;

public sealed class Customer
{
    public CustomerId Id { get; private set; }
    public CustomerName Name { get; private set; }
}

