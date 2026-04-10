namespace YuyoDev.Application.Interfaces;

using YuyoDev.Application.DTOs.Products;
using YuyoDev.Domain.Shared;

public interface IProductService
{
    Task<Result<Guid>> CreateProductAsync(CreateProductDto request, CancellationToken cancellationToken);
    Task<Result<ProductDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}