namespace YuyoDev.Domain.Entities.Catalog;

public class Tag
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty; // Ej: "Oferta", "Nuevo"

    protected Tag() { }

    public static Tag Create(string name) => new Tag { Name = name };
}