namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Entities.Catalog;

public class Product : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; } = null!;

    public Guid BrandId { get; private set; }
    public virtual Brand Brand { get; private set; } = null!;

    private readonly List<ProductVariant> _variants = new();
    public virtual IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    public string TenantId { get; set; } = string.Empty;

    // Constructor vacío requerido por Entity Framework
    protected Product() { }

    // Factory Method para crear instancias válidas
    public static Product Create(string name, string description, Guid categoryId, Guid brandId)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CategoryId = categoryId,
            BrandId = brandId
        };
    }

    // Método de negocio para actualizar información
    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    // Método de negocio para agregar variantes cuidando el encapsulamiento
    public void AddVariant(ProductVariant variant)
    {
        if (variant == null) throw new ArgumentNullException(nameof(variant));
        _variants.Add(variant);
    }
}