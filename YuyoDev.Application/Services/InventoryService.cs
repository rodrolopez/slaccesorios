namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Logistics;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;

    // --- ESTE ES EL CONSTRUCTOR QUE FALTABA ---
    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task AddStockAsync(Guid productVariantId, int quantity, decimal unitCost, string notes, CancellationToken cancellationToken)
    {
        var variant = await _repository.GetVariantByIdAsync(productVariantId, cancellationToken);
        if (variant == null) throw new Exception("Variante no encontrada");

        variant.UpdateStock(quantity);
        var movement = StockMovement.Create(productVariantId, "Ingreso", quantity, unitCost, notes);

        await _repository.AddStockMovementAsync(movement, cancellationToken);
        await _repository.UpdateVariantAsync(variant, cancellationToken);
    }

    public async Task ReduceStockAsync(Guid productVariantId, int quantity, string notes, CancellationToken cancellationToken)
    {
        var variant = await _repository.GetVariantByIdAsync(productVariantId, cancellationToken);
        if (variant == null) throw new Exception("Variante no encontrada");
        if (variant.Stock < quantity) throw new Exception("Stock insuficiente");

        variant.UpdateStock(-quantity);
        var movement = StockMovement.Create(productVariantId, "Egreso", -quantity, variant.CostPrice, notes);

        await _repository.AddStockMovementAsync(movement, cancellationToken);
        await _repository.UpdateVariantAsync(variant, cancellationToken);
    }

    public async Task<IEnumerable<StockMovement>> GetStockHistoryAsync(Guid productVariantId, CancellationToken cancellationToken)
    {
        return await _repository.GetStockHistoryAsync(productVariantId, cancellationToken);
    }

    public async Task<bool> CheckAvailabilityAsync(Guid productVariantId, int requestedQuantity, CancellationToken cancellationToken)
    {
        var stock = await _repository.GetCurrentStockAsync(productVariantId, cancellationToken);
        return stock >= requestedQuantity;
    }
}