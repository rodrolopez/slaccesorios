namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.CMS;
using YuyoDev.Infrastructure.Persistence;

public class StoreConfigurationRepository : IStoreConfigurationRepository
{
    private readonly ApplicationDbContext _context;

    public StoreConfigurationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StoreConfiguration?> GetConfigurationAsync(CancellationToken cancellationToken)
    {
        return await _context.StoreConfigurations.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddConfigurationAsync(StoreConfiguration configuration, CancellationToken cancellationToken)
    {
        _context.StoreConfigurations.Add(configuration);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateConfigurationAsync(StoreConfiguration configuration, CancellationToken cancellationToken)
    {
        _context.StoreConfigurations.Update(configuration);
        await _context.SaveChangesAsync(cancellationToken);
    }
}