namespace YuyoDev.Domain.Entities.Catalog;

public class Brand
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Brand() { }

    public static Brand Create(string name, string? logoUrl = null)
    {
        return new Brand { Name = name, LogoUrl = logoUrl };
    }
}