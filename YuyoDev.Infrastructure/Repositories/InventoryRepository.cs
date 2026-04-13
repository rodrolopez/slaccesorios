namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Logistics;
using YuyoDev.Infrastructure.Persistence;

public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductVariant?> GetVariantByIdAsync(Guid variantId, CancellationToken cancellationToken)
    {
        return await _context.ProductVariants.FindAsync(new object[] { variantId }, cancellationToken);
    }

    public async Task AddStockMovementAsync(StockMovement movement, CancellationToken cancellationToken)
    {
        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockMovement>> GetStockHistoryAsync(Guid variantId, CancellationToken cancellationToken)
    {
        return await _context.StockMovements.Where(sm => sm.ProductVariantId == variantId).OrderByDescending(sm => sm.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task UpdateVariantAsync(ProductVariant variant, CancellationToken cancellationToken)
    {
        _context.ProductVariants.Update(variant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetCurrentStockAsync(Guid variantId, CancellationToken cancellationToken)
    {
        return await _context.ProductVariants.Where(pv => pv.Id == variantId).Select(pv => pv.Stock).FirstOrDefaultAsync(cancellationToken);
    }
}