namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return _context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
    }

    public Task<bool> BrandExistsAsync(Guid brandId, CancellationToken cancellationToken)
    {
        return _context.Brands.AnyAsync(b => b.Id == brandId, cancellationToken);
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<Product?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}