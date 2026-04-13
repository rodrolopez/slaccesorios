namespace YuyoDev.Domain.Entities.Catalog;

public class Category
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty; // Para la URL: "aros-acero"
    public string? Description { get; private set; }

    // Para subcategorías (Ej: Categoría Padre "Aros", Hija "Acero Quirúrgico")
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> SubCategories { get; private set; } = new List<Category>();

    public bool IsActive { get; private set; } = true;

    protected Category() { }

    public static Category Create(string name, string slug, Guid? parentCategoryId = null)
    {
        return new Category
        {
            Name = name,
            Slug = slug,
            ParentCategoryId = parentCategoryId
        };
    }
}