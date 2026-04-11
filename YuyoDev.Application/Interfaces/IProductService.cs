namespace YuyoDev.Application.Interfaces;

using YuyoDev.Application.DTOs.Products;
using YuyoDev.Domain.Shared;
using YuyoDev.Application.DTOs;

public interface IProductService
{
    Task<Result<Guid>> CreateProductAsync(CreateProductDto request, CancellationToken cancellationToken);
    Task<Result<ProductDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<PagedResultDto<ProductDto>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
}