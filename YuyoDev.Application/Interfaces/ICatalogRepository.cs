namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Catalog;

public interface ICatalogRepository
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken);
    Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(string? searchTerm, Guid? categoryId, Guid? brandId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Product?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken);
}