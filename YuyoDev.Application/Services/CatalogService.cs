namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Catalog;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;

    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetActiveCategoriesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetActiveBrandsAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(
        string? searchTerm, Guid? categoryId, Guid? brandId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _repository.SearchProductsAsync(searchTerm, categoryId, brandId, pageNumber, pageSize, cancellationToken);
    }

    public async Task<Product?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _repository.GetProductDetailAsync(productId, cancellationToken);
    }
}