namespace YuyoDev.Domain.Entities.Shipping;

public class ShippingMethod
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    // Ej: "Retiro en Local", "Envío por Moto", "Andreani"
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // Precio base del envío. Si es "Retiro en Local", sería 0.
    public decimal BasePrice { get; private set; }

    // Ej: "1 a 3 días hábiles"
    public string EstimatedDeliveryTime { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    protected ShippingMethod() { }

    public static ShippingMethod Create(string name, string? description, decimal basePrice, string estimatedDeliveryTime)
    {
        return new ShippingMethod
        {
            Name = name,
            Description = description,
            BasePrice = basePrice,
            EstimatedDeliveryTime = estimatedDeliveryTime
        };
    }
}