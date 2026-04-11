namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class ProductVariant : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; } = null!;

    public string SKU { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public string ModelName { get; private set; } = string.Empty;

    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public string TenantId { get; set; } = string.Empty;

    protected ProductVariant() { }

    public static ProductVariant Create(Guid productId, string sku, string color, string modelName, decimal price, int initialStock)
    {
        // Acá podés meter reglas de negocio duras. Ej:
        if (price < 0) throw new ArgumentException("El precio no puede ser negativo");

        return new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            SKU = sku,
            Color = color,
            ModelName = modelName,
            Price = price,
            Stock = initialStock
        };
    }

    public void UpdateStock(int quantityToAdd)
    {
        Stock += quantityToAdd;
        if (Stock < 0) Stock = 0; // Previene stock negativo por accidente
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("El precio no puede ser negativo");
        Price = newPrice;
    }
}