namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Catalog;
using YuyoDev.Infrastructure.Persistence;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken)
    {
        return await _context.Brands.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(
        string? searchTerm, Guid? categoryId, Guid? brandId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
            .Include(p => p.ProductImages)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.OrderByDescending(p => p.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return (products, totalCount);
    }

    public async Task<Product?> GetProductDetailAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }
}