namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Shipping;

public interface IShippingRepository
{
    Task<IEnumerable<ShippingMethod>> GetActiveShippingMethodsAsync(CancellationToken cancellationToken);
    Task<ShippingMethod?> GetMethodByIdAsync(Guid methodId, CancellationToken cancellationToken);

    // Busca si existe un recargo adicional para una zona específica (ej: "Gran San Juan")
    Task<ShippingZone?> GetZoneByMethodAndNameAsync(Guid methodId, string zoneName, CancellationToken cancellationToken);
}