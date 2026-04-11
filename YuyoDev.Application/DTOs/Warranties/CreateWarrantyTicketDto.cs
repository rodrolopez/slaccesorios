namespace YuyoDev.Application.DTOs.Warranties;

public class CreateWarrantyTicketDto
{
    public Guid OrderItemId { get; set; }
    public string IssueDescription { get; set; } = string.Empty;
}