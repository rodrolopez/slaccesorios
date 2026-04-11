namespace YuyoDev.Application.DTOs.Warranties;
using YuyoDev.Domain.Enums;

public class WarrantyTicketDto
{
    public Guid Id { get; set; }
    public Guid OrderItemId { get; set; }
    public string IssueDescription { get; set; } = string.Empty;
    public string? ResolutionNotes { get; set; }
    public string Status { get; set; } = string.Empty; // Lo mandamos como string para el frontend
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}