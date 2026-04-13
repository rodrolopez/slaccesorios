namespace YuyoDev.Domain.Entities.Sales;

using YuyoDev.Domain.Entities;

public class Wishlist
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Guid ProductId { get; private set; }

    public virtual Product? Product { get; private set; }

    public DateTime AddedAt { get; private set; } = DateTime.UtcNow;

    protected Wishlist() { }

    public static Wishlist Create(string userId, Guid productId)
    {
        return new Wishlist
        {
            UserId = userId,
            ProductId = productId
        };
    }
}