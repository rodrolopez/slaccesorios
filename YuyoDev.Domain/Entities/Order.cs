namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Enums;

public class Order : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    private readonly List<OrderItem> _items = new();
    public virtual IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public string TenantId { get; set; } = string.Empty;

    protected Order() { }

    public static Order Create(string userId)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            TotalAmount = 0
        };
    }

    public void AddItem(Guid productVariantId, int quantity, decimal unitPrice)
    {
        var item = OrderItem.Create(productVariantId, quantity, unitPrice);
        _items.Add(item);
        TotalAmount += (quantity * unitPrice); // Se recalcula el total automáticamente
    }

    public void MarkAsPaid()
    {
        PaymentStatus = PaymentStatus.Approved;
        Status = OrderStatus.Processing;
    }
}