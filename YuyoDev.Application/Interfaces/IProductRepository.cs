namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;

public interface IProductRepository
{
    Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<bool> BrandExistsAsync(Guid brandId, CancellationToken cancellationToken);
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<(List<Product> Products, int TotalCount)> GetAllProductsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
}