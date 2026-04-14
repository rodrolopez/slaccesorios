namespace YuyoDev.AdminPanel.Models;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    // Usamos strings para el MVP, luego lo atamos a los IDs reales de Categorías
    public string CategoryName { get; set; } = "Accesorios";
}