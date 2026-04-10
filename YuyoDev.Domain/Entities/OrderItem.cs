using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Enums;

namespace YuyoDev.Domain.Entities;
public class OrderItem : IMustHaveTenant
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;

    public Guid ProductVariantId { get; set; }
    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Congelamos el precio al momento de la compra

    public string TenantId { get; set; } = string.Empty;
}