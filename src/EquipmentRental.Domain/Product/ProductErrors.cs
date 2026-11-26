using EquipmentRental.Domain.Shared;

namespace EquipmentRental.Domain.Product;

public static class ProductErrors
{
    public static readonly Error IdRequired =
        new("Product.IdRequired", "Id is required.");

    public static readonly Error NameRequired =
        new("Product.NameRequired", "Name is required.");

    public static readonly Error DescriptionRequired =
        new("Product.DescriptionRequired", "Description is required.");

    public static readonly Error DescriptionTooLong =
        new("Product.DescriptionTooLong", "Description must be under 100 characters.");

    public static readonly Error CategoryRequired =
        new("Product.CategoryRequired", "Category is required.");

    public static readonly Error RateRequired =
        new("Product.RateRequired", "Rental rate is required and must be greater than zero.");

    public static readonly Error PricingBasisRequired =
        new("Product.PricingBasisRequired", "Pricing basis is required.");

    public static readonly Error ManufacturerRequired =
        new("Product.ManufacturerRequired", "Manufacturer is required.");

    public static readonly Error ModelRequired =
        new("Product.ModelRequired", "Model is required.");

    public static readonly Error HandlingNotesRequired =
        new("Product.HandlingNotesRequired", "Handling notes are required.");
}