namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Shipping;

public class ShippingService : IShippingService
{
    private readonly IShippingRepository _repository;

    public ShippingService(IShippingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ShippingMethod>> GetActiveShippingMethodsAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetActiveShippingMethodsAsync(cancellationToken);
    }

    public async Task<decimal> CalculateShippingCostAsync(Guid shippingMethodId, string zoneName, CancellationToken cancellationToken)
    {
        var method = await _repository.GetMethodByIdAsync(shippingMethodId, cancellationToken);
        if (method == null || !method.IsActive) throw new Exception("Método de envío no disponible.");

        decimal finalCost = method.BasePrice;

        // Si el cliente indicó una zona, verificamos si hay un costo extra
        if (!string.IsNullOrWhiteSpace(zoneName))
        {
            var zone = await _repository.GetZoneByMethodAndNameAsync(shippingMethodId, zoneName, cancellationToken);
            if (zone != null)
            {
                finalCost += zone.AdditionalCost;
            }
        }

        return finalCost;
    }
}