namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class Category : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TenantId { get; set; } = string.Empty;
}
