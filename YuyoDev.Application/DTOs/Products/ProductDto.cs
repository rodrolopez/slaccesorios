namespace YuyoDev.Application.DTOs.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Devolvemos los nombres directamente para que el frontend no tenga que hacer malabares
    public string CategoryName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
}