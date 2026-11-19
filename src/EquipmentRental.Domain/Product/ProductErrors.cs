using EquipmentRental.Domain.Common;

namespace EquipmentRental.Domain.Product;

public static class ProductErrors
{
    public static readonly Error IdRequired =
        new("Product.IdRequired", "Id is required.");

    public static readonly Error NameRequired =
        new("Product.NameRequired", "Name is required.");
}