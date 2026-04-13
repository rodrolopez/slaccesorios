namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Catalog;

public interface ICatalogService
{
    // Categorías y Marcas para los filtros del frontend
    Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken);

    // El buscador principal de productos (con paginación para no explotar la memoria)
    Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(
        string? searchTerm,
        Guid? categoryId,
        Guid? brandId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    // Detalle de un producto específico cuando el cliente hace clic
    Task<Product?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken);
}