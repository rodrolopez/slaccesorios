namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Sales;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;

    public CartService(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShoppingCart> GetCartAsync(string? userId, string? sessionId, CancellationToken cancellationToken)
    {
        ShoppingCart? cart = null;

        if (!string.IsNullOrEmpty(userId))
            cart = await _repository.GetCartByUserIdAsync(userId, cancellationToken);
        else if (!string.IsNullOrEmpty(sessionId))
            cart = await _repository.GetCartBySessionIdAsync(sessionId, cancellationToken);

        // Si no existe, creamos uno vacío
        if (cart == null)
        {
            cart = ShoppingCart.Create(userId, sessionId);
            await _repository.AddCartAsync(cart, cancellationToken);
        }

        return cart;
    }

    public async Task<ShoppingCart> AddItemToCartAsync(string? userId, string? sessionId, Guid productVariantId, int quantity, decimal currentPrice, CancellationToken cancellationToken)
    {
        var cart = await GetCartAsync(userId, sessionId, cancellationToken);

        // Creamos el ítem y lo agregamos al carrito usando los métodos del dominio
        var item = CartItem.Create(cart.Id, productVariantId, quantity, currentPrice);
        cart.AddItem(item);

        await _repository.UpdateCartAsync(cart, cancellationToken);
        return cart;
    }

    public async Task RemoveItemFromCartAsync(Guid cartItemId, CancellationToken cancellationToken)
    {
        var item = await _repository.GetCartItemByIdAsync(cartItemId, cancellationToken);
        if (item != null)
        {
            await _repository.RemoveCartItemAsync(item, cancellationToken);
        }
    }

    public async Task ClearCartAsync(Guid shoppingCartId, CancellationToken cancellationToken)
    {
        // Traemos el carrito por ID, pero como nuestro repo busca por User o Session,
        // acá por simplicidad podemos asumir que se limpiará cuando se procese la orden.
        // Lo dejamos preparado para implementar más adelante en la integración con Checkout.
        throw new NotImplementedException("Se implementará en el flujo de Checkout");
    }
}