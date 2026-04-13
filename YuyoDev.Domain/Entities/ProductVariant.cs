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

    public static ProductVariant Create(Guid productId, string sku, string color, string modelName, decimal price, int initialStock, decimal costPrice, decimal sellingPrice)
    {
        if (price < 0) throw new ArgumentException("El precio no puede ser negativo");

        return new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            SKU = sku,
            Color = color,
            ModelName = modelName,
            Price = price,
            Stock = initialStock,
            CostPrice = costPrice, // Ahora sí sabe de dónde viene
            SellingPrice = sellingPrice
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

    public decimal CostPrice { get; private set; } // Lo que nos cobra el proveedor
    public decimal SellingPrice { get; private set; } // A lo que lo vendemos

// Propiedad calculada al vuelo (no se guarda en DB, se calcula sola)
    public decimal ProfitMarginPercentage =>
        CostPrice > 0 ? ((SellingPrice - CostPrice) / CostPrice) * 100 : 0;
}