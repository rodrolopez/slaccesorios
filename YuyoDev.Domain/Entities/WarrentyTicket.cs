namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;
using YuyoDev.Domain.Enums;

public class WarrantyTicket : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public Guid OrderItemId { get; private set; }
    public virtual OrderItem OrderItem { get; private set; } = null!;

    public string IssueDescription { get; private set; } = string.Empty;
    public string? ResolutionNotes { get; private set; }

    public WarrantyStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }

    public string TenantId { get; set; } = string.Empty;

    protected WarrantyTicket() { }

    public static WarrantyTicket Create(Guid orderItemId, string issueDescription)
    {
        if (string.IsNullOrWhiteSpace(issueDescription))
            throw new ArgumentException("La descripción del problema es obligatoria.");

        return new WarrantyTicket
        {
            Id = Guid.NewGuid(),
            OrderItemId = orderItemId,
            IssueDescription = issueDescription,
            Status = WarrantyStatus.Open,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateStatus(WarrantyStatus newStatus, string? resolutionNotes = null)
    {
        Status = newStatus;
        if (!string.IsNullOrWhiteSpace(resolutionNotes))
        {
            ResolutionNotes = resolutionNotes;
        }

        if (newStatus == WarrantyStatus.Resolved || newStatus == WarrantyStatus.Rejected)
        {
            ResolvedAt = DateTime.UtcNow;
        }
    }
}