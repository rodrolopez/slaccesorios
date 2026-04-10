namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class Product : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public string TenantId { get; set; } = string.Empty;
}
