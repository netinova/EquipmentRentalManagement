using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Customer;

public static class CustomerErrors
{
    public static readonly Error FirstNameRequired =
        new("Customer.FirstNameRequired", "First name is required.");

    public static readonly Error LastNameRequired =
        new("Customer.LastNameRequired", "Last name is required.");

    public static readonly Error IdRequired =
        new("Customer.IdRequired", "Id is required.");
}