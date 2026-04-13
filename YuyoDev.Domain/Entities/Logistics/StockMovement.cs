namespace YuyoDev.Domain.Entities.Logistics;

using YuyoDev.Domain.Entities; // Para acceder a ProductVariant

public class StockMovement
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductVariantId { get; private set; }

    // Puede ser "Ingreso", "Egreso", etc.
    public string MovementType { get; private set; } = string.Empty;

    // Cantidad (Positiva si entra, negativa si sale)
    public int Quantity { get; private set; }

    public decimal UnitCost { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public virtual ProductVariant? ProductVariant { get; private set; }

    protected StockMovement() { }

    public static StockMovement Create(Guid productVariantId, string movementType, int quantity, decimal unitCost, string? notes)
    {
        return new StockMovement
        {
            ProductVariantId = productVariantId,
            MovementType = movementType,
            Quantity = quantity,
            UnitCost = unitCost,
            Notes = notes
        };
    }
}