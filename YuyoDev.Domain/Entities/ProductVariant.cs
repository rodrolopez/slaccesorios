namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;
public class ProductVariant : IMustHaveTenant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public string Sku { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Stock { get; set; }

    public string TenantId { get; set; } = string.Empty;
}