namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs;
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

        // ACÁ ESTÁ LA MAGIA: Usamos el Factory Method en lugar de hacer "new Product { ... }"
        var product = Product.Create(request.Name, request.Description, request.CategoryId, request.BrandId);

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

    // LA IMPLEMENTACIÓN DEL GET ALL PAGINADO
    public async Task<Result<PagedResultDto<ProductDto>>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _repository.GetAllProductsAsync(pageNumber, pageSize, searchTerm, cancellationToken);

        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CategoryName = p.Category?.Name ?? "Sin categoría",
            BrandName = p.Brand?.Name ?? "Sin marca"
        }).ToList();

        var pagedResult = new PagedResultDto<ProductDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResultDto<ProductDto>>.Success(pagedResult);
    }
}