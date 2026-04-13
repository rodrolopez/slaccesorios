namespace YuyoDev.Domain.Entities.Sales;

public class ShoppingCart
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string? UserId { get; private set; }
    public string? SessionId { get; private set; }

    private readonly List<CartItem> _items = new();
    public virtual IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public DateTime LastAccessedAt { get; private set; } = DateTime.UtcNow;

    protected ShoppingCart() { }

    public static ShoppingCart Create(string? userId, string? sessionId)
    {
        return new ShoppingCart
        {
            UserId = userId,
            SessionId = sessionId
        };
    }

    public void AddItem(CartItem item)
    {
        _items.Add(item);
        LastAccessedAt = DateTime.UtcNow;
    }
}