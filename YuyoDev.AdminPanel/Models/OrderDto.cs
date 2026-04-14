namespace YuyoDev.AdminPanel.Models;

public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty; // Ej: "Pending", "Paid", "Shipped"
}