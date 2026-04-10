using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Enums;


namespace YuyoDev.Domain.Entities;

public class WarrantyTicket : IMustHaveTenant
{
    public Guid Id { get; set; }

    // Vinculamos la garantía a un ítem específico comprado
    public Guid OrderItemId { get; set; }
    public virtual OrderItem OrderItem { get; set; } = null!;

    public string IssueDescription { get; set; } = string.Empty;
    public string? ResolutionNotes { get; set; }

    public WarrantyStatus Status { get; set; } = WarrantyStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }

    public string TenantId { get; set; } = string.Empty;
}