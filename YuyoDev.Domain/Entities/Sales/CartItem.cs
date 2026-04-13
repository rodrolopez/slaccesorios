namespace YuyoDev.Domain.Entities.Sales;

using YuyoDev.Domain.Entities;

public class CartItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ShoppingCartId { get; private set; }
    public ShoppingCart ShoppingCart { get; private set; } = null!;

    public Guid ProductVariantId { get; private set; }

    public virtual ProductVariant? ProductVariant { get; private set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    protected CartItem() { }

    public static CartItem Create(Guid shoppingCartId, Guid productVariantId, int quantity, decimal unitPrice)
    {
        return new CartItem
        {
            ShoppingCartId = shoppingCartId,
            ProductVariantId = productVariantId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}