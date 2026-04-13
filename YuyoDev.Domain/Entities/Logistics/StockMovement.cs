namespace YuyoDev.Domain.Entities.Logistics;

using YuyoDev.Domain.Entities; // Para acceder a ProductVariant si está en la raíz

public class StockMovement
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductVariantId { get; private set; }

    // Puede ser "IngresoProveedor", "Venta", "DevolucionGarantia", "AjusteManual"
    public string MovementType { get; private set; } = string.Empty;

    // Cantidad (Positiva si entra, negativa si sale)
    public int Quantity { get; private set; }

    // Si es un ingreso, a qué costo entró esta tanda específica
    public decimal UnitCost { get; private set; }

    public string? Notes { get; private set; } // Ej: "Remito #1234"
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Relación de navegación
    public ProductVariant? ProductVariant { get; private set; }

    // Constructor para Clean Architecture
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