namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class Brand : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
}