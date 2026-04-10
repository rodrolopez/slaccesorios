using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Enums;

namespace YuyoDev.Domain.Entities;

public class Order : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty; // Enlace con el ApplicationUser (Identity)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    // Detalles de la compra
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public string TenantId { get; set; } = string.Empty;
}