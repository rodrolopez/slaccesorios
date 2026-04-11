namespace YuyoDev.Domain.Entities;

using YuyoDev.Domain.Abstractions;

public class Brand : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;

    protected Brand() { }

    public static Brand Create(string name)
    {
        return new Brand
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}