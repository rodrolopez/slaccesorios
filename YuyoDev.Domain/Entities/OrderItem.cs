namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class OrderItem : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public virtual Order Order { get; private set; } = null!;

    public Guid ProductVariantId { get; private set; }
    public virtual ProductVariant ProductVariant { get; private set; } = null!;

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public string TenantId { get; set; } = string.Empty;

    protected OrderItem() { }

    public static OrderItem Create(Guid productVariantId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
        if (unitPrice < 0) throw new ArgumentException("El precio no puede ser negativo.");

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductVariantId = productVariantId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}