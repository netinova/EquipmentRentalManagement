using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Customer;

public sealed record CustomerName
{
    public string FirstName { get; }
    public string LastName { get; }

    private CustomerName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<CustomerName> Create(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return CustomerErrors.FirstNameRequired;

        if (string.IsNullOrWhiteSpace(lastName))
            return CustomerErrors.LastNameRequired;


        firstName = firstName.Trim();
        lastName = lastName.Trim();

        return new CustomerName(firstName, lastName);
    }
}
