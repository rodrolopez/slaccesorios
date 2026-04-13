namespace YuyoDev.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Marketing;
using YuyoDev.Infrastructure.Persistence;

public class MarketingRepository : IMarketingRepository
{
    private readonly ApplicationDbContext _context;

    public MarketingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountCoupon?> GetCouponByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await _context.DiscountCoupons
            .FirstOrDefaultAsync(c => c.Code == code.ToUpper(), cancellationToken);
    }

    public async Task UpdateCouponAsync(DiscountCoupon coupon, CancellationToken cancellationToken)
    {
        _context.DiscountCoupons.Update(coupon);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddReviewAsync(ProductReview review, CancellationToken cancellationToken)
    {
        _context.ProductReviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductReview>> GetApprovedReviewsByProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.ProductReviews
            .Where(r => r.ProductId == productId && r.IsApproved)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken)
    {
        return await _context.ProductReviews
            .Where(r => !r.IsApproved)
            .OrderBy(r => r.CreatedAt) // Las más viejas primero para moderar
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductReview?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken)
    {
        return await _context.ProductReviews.FindAsync(new object[] { reviewId }, cancellationToken);
    }

    public async Task UpdateReviewAsync(ProductReview review, CancellationToken cancellationToken)
    {
        _context.ProductReviews.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
    }
}