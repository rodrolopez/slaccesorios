namespace YuyoDev.Domain.Entities.Catalog;

public class ProductImage
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; } // O ProductVariantId, según tu base
    public string ImageUrl { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; } // Para saber cuál es la foto de portada
    public int DisplayOrder { get; private set; }

    protected ProductImage() { }

    public static ProductImage Create(Guid productId, string imageUrl, bool isPrimary, int displayOrder)
    {
        return new ProductImage
        {
            ProductId = productId,
            ImageUrl = imageUrl,
            IsPrimary = isPrimary,
            DisplayOrder = displayOrder
        };
    }
}