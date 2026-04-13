namespace YuyoDev.Domain.Entities.Shipping;

public class ShippingZone
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ShippingMethodId { get; private set; }
    public virtual ShippingMethod? ShippingMethod { get; private set; }

    // Ej: "San Juan Capital", "Gran San Juan", "Resto del País"
    public string ZoneName { get; private set; } = string.Empty;

    // Un costo extra que se suma al BasePrice del método
    public decimal AdditionalCost { get; private set; }

    protected ShippingZone() { }

    public static ShippingZone Create(Guid shippingMethodId, string zoneName, decimal additionalCost)
    {
        return new ShippingZone
        {
            ShippingMethodId = shippingMethodId,
            ZoneName = zoneName,
            AdditionalCost = additionalCost
        };
    }
}