namespace YuyoDev.Domain.Entities.Sales;

using YuyoDev.Domain.Entities;

public class PaymentTransaction
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid OrderId { get; private set; }
    public virtual Order? Order { get; private set; }

    public decimal Amount { get; private set; }

    // Ej: "MercadoPago", "Transferencia", "Stripe"
    public string Provider { get; private set; } = string.Empty;

    // Ej: "Approved", "Rejected", "Pending"
    public string Status { get; private set; } = string.Empty;

    // El ID de transacción que te devuelve MercadoPago o el banco
    public string? ExternalReference { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; private set; }

    protected PaymentTransaction() { }

    public static PaymentTransaction Create(Guid orderId, decimal amount, string provider, string status, string? externalReference)
    {
        return new PaymentTransaction
        {
            OrderId = orderId,
            Amount = amount,
            Provider = provider,
            Status = status,
            ExternalReference = externalReference
        };
    }

    public void UpdateStatus(string newStatus, DateTime processedAt)
    {
        Status = newStatus;
        ProcessedAt = processedAt;
    }
}