namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Sales;

public interface ICartService
{
    Task<ShoppingCart> GetCartAsync(string? userId, string? sessionId, CancellationToken cancellationToken);
    Task<ShoppingCart> AddItemToCartAsync(string? userId, string? sessionId, Guid productVariantId, int quantity, decimal currentPrice, CancellationToken cancellationToken);
    Task RemoveItemFromCartAsync(Guid cartItemId, CancellationToken cancellationToken);
    Task ClearCartAsync(Guid shoppingCartId, CancellationToken cancellationToken);
}