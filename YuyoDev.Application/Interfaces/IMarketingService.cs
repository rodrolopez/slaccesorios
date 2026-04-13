namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.Marketing;

public interface IMarketingService
{
    // Lógica de Cupones
    Task<DiscountCoupon?> ValidateCouponAsync(string code, CancellationToken cancellationToken);
    Task ApplyCouponAsync(string code, CancellationToken cancellationToken);

    // Lógica de Reseñas
    Task AddProductReviewAsync(Guid productId, string userId, string customerName, int rating, string comment, CancellationToken cancellationToken);
    Task<IEnumerable<ProductReview>> GetApprovedReviewsForProductAsync(Guid productId, CancellationToken cancellationToken);

    Task<IEnumerable<ProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken);
    Task ApproveReviewAsync(Guid reviewId, CancellationToken cancellationToken);
}