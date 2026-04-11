namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class Category : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string TenantId { get; set; } = string.Empty;

    protected Category() { }

    public static Category Create(string name, string? description = null)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }
}