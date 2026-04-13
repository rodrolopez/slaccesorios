namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Shipping;
using YuyoDev.Infrastructure.Persistence;

public class ShippingRepository : IShippingRepository
{
    private readonly ApplicationDbContext _context;

    public ShippingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShippingMethod>> GetActiveShippingMethodsAsync(CancellationToken cancellationToken)
    {
        return await _context.ShippingMethods
            .Where(sm => sm.IsActive)
            .OrderBy(sm => sm.BasePrice) // Ordenamos del más barato al más caro
            .ToListAsync(cancellationToken);
    }

    public async Task<ShippingMethod?> GetMethodByIdAsync(Guid methodId, CancellationToken cancellationToken)
    {
        return await _context.ShippingMethods.FindAsync(new object[] { methodId }, cancellationToken);
    }

    public async Task<ShippingZone?> GetZoneByMethodAndNameAsync(Guid methodId, string zoneName, CancellationToken cancellationToken)
    {
        return await _context.ShippingZones
            .FirstOrDefaultAsync(sz => sz.ShippingMethodId == methodId && sz.ZoneName.ToLower() == zoneName.ToLower(), cancellationToken);
    }
}