namespace YuyoDev.Domain.Entities.Marketing;

using YuyoDev.Domain.Entities;

public class ProductReview
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ProductId { get; private set; }
    public virtual Product? Product { get; private set; }

    public string UserId { get; private set; } = string.Empty; // El ID de Identity
    public string CustomerName { get; private set; } = string.Empty; // Para mostrar "Rodrigo D."

    public int Rating { get; private set; } // Estrellas del 1 al 5
    public string? Comment { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Fundamental: arranca en false para que el Admin apruebe la reseña y evitar spam
    public bool IsApproved { get; private set; } = false;

    protected ProductReview() { }

    public static ProductReview Create(Guid productId, string userId, string customerName, int rating, string? comment)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("El rating debe estar entre 1 y 5");

        return new ProductReview
        {
            ProductId = productId,
            UserId = userId,
            CustomerName = customerName,
            Rating = rating,
            Comment = comment
        };
    }
}