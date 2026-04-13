namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Logistics;

public interface IInventoryService
{
    // Sumar o restar stock, dejando registro inmutable en StockMovement
    Task AddStockAsync(Guid productVariantId, int quantity, decimal unitCost, string notes, CancellationToken cancellationToken);
    Task ReduceStockAsync(Guid productVariantId, int quantity, string notes, CancellationToken cancellationToken);

    // Para el dashboard financiero: historial de movimientos
    Task<IEnumerable<StockMovement>> GetStockHistoryAsync(Guid productVariantId, CancellationToken cancellationToken);

    // Chequeo rápido antes de dejar que un cliente agregue al carrito
    Task<bool> CheckAvailabilityAsync(Guid productVariantId, int requestedQuantity, CancellationToken cancellationToken);
}