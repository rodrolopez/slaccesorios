namespace YuyoDev.AdminPanel.Models;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CategoryName { get; set; } = "Sin categoría";
    public string BrandName { get; set; } = "Sin marca";
}