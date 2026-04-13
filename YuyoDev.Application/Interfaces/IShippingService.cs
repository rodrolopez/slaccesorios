namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Shipping;

public interface IShippingService
{
    Task<IEnumerable<ShippingMethod>> GetActiveShippingMethodsAsync(CancellationToken cancellationToken);
    Task<decimal> CalculateShippingCostAsync(Guid shippingMethodId, string zoneName, CancellationToken cancellationToken);
}