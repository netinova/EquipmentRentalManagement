using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Customer;

public static class CustomerErrors
{
    public static readonly Error FirstNameRequired =
        new("Customers.FirstNameRequired", "First name is required.");

    public static readonly Error LastNameRequired =
        new("Customers.LastNameRequired", "Last name is required.");
}