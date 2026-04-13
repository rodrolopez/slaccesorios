namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Marketing;

public interface IMarketingRepository
{
    // Cupones
    Task<DiscountCoupon?> GetCouponByCodeAsync(string code, CancellationToken cancellationToken);
    Task UpdateCouponAsync(DiscountCoupon coupon, CancellationToken cancellationToken);

    // Reseñas
    Task AddReviewAsync(ProductReview review, CancellationToken cancellationToken);
    Task<IEnumerable<ProductReview>> GetApprovedReviewsByProductAsync(Guid productId, CancellationToken cancellationToken);
    Task<IEnumerable<ProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken);
    Task<ProductReview?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken);
    Task UpdateReviewAsync(ProductReview review, CancellationToken cancellationToken);
}