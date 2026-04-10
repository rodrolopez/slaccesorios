namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Products;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Shared;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> CreateProductAsync(CreateProductDto request, CancellationToken cancellationToken)
    {
        var categoryExists = await _repository.CategoryExistsAsync(request.CategoryId, cancellationToken);
        if (!categoryExists) return Result<Guid>.Failure("La categoría no existe.");

        var brandExists = await _repository.BrandExistsAsync(request.BrandId, cancellationToken);
        if (!brandExists) return Result<Guid>.Failure("La marca no existe.");

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId
        };

        await _repository.AddProductAsync(product, cancellationToken);

        return Result<Guid>.Success(product.Id);
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductWithDetailsAsync(id, cancellationToken);

        if (product is null) return Result<ProductDto>.Failure("Producto no encontrado.");

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryName = product.Category?.Name ?? "Sin categoría",
            BrandName = product.Brand?.Name ?? "Sin marca"
        };

        return Result<ProductDto>.Success(dto);
    }
}