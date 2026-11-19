namespace EquipmentRental.Domain.Product;

public class Product
{
    public ProductId Id { get; private set; }
    public ProductName Name { get; private set; }
    public RentAvailablity RentAvailablity { get; private set; }
}