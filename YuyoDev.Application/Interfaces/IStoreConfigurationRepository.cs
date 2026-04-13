namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.CMS;

public interface IStoreConfigurationRepository
{
    // Normalmente hay una sola configuración por Tenant/Tienda
    Task<StoreConfiguration?> GetConfigurationAsync(CancellationToken cancellationToken);
    Task AddConfigurationAsync(StoreConfiguration configuration, CancellationToken cancellationToken);
    Task UpdateConfigurationAsync(StoreConfiguration configuration, CancellationToken cancellationToken);
}