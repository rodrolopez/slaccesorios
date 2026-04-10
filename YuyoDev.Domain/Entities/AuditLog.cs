using YuyoDev.Domain.Interfaces;
namespace YuyoDev.Domain.Entities;

public class AuditLog : IMustHaveTenant
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Ej: "Delete User", "Create Product"
    public string EntityName { get; set; } = string.Empty; // Ej: "ApplicationUser"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = string.Empty; // JSON o texto con info extra
    public string TenantId { get; set; } = string.Empty; // Para separar los logs por cliente
}