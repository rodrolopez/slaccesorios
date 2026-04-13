namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Logistics;

public interface IInventoryRepository
{
    Task<ProductVariant?> GetVariantByIdAsync(Guid variantId, CancellationToken cancellationToken);
    Task AddStockMovementAsync(StockMovement movement, CancellationToken cancellationToken);
    Task<IEnumerable<StockMovement>> GetStockHistoryAsync(Guid variantId, CancellationToken cancellationToken);
    Task UpdateVariantAsync(ProductVariant variant, CancellationToken cancellationToken);
    Task<int> GetCurrentStockAsync(Guid variantId, CancellationToken cancellationToken);
}