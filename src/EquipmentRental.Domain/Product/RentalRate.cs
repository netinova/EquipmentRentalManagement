using EquipmentRental.Domain.Abstractions;

namespace EquipmentRental.Domain.Product;

public enum PricingBasis
{
    Hourly,
    Daily,
    Weekly,
    Monthly
}

public sealed record RentalRate
{
    public decimal Rate { get; }
    public PricingBasis PricingBasis { get; }

    private RentalRate(decimal rate, PricingBasis pricingBasis)
    {
        Rate = rate;
        PricingBasis = pricingBasis;
    }

    public static Result<RentalRate> Create(decimal? rate, PricingBasis? pricingBasis)
    {
        if (rate is null || rate <= 0)
            return ProductErrors.RateRequired;

        if (pricingBasis is null)
            return ProductErrors.PricingBasisRequired;

        return new RentalRate(rate.Value, pricingBasis.Value);
    }
}
