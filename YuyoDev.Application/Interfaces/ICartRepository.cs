namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Sales;

public interface ICartRepository
{
    Task<ShoppingCart?> GetCartByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<ShoppingCart?> GetCartBySessionIdAsync(string sessionId, CancellationToken cancellationToken);
    Task AddCartAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task UpdateCartAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task DeleteCartAsync(ShoppingCart cart, CancellationToken cancellationToken);

    Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken);
    Task RemoveCartItemAsync(CartItem item, CancellationToken cancellationToken);
}