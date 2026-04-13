namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.Marketing;

public class MarketingService : IMarketingService
{
    private readonly IMarketingRepository _repository;

    public MarketingService(IMarketingRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountCoupon?> ValidateCouponAsync(string code, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetCouponByCodeAsync(code, cancellationToken);

        // Reglas de negocio: Existe, está activo y no venció
        if (coupon == null || !coupon.IsActive || coupon.ExpiryDate < DateTime.UtcNow)
            return null;

        return coupon;
    }

    public async Task ApplyCouponAsync(string code, CancellationToken cancellationToken)
    {
        var coupon = await ValidateCouponAsync(code, cancellationToken);
        if (coupon == null) throw new Exception("Cupón inválido o expirado.");

        // El dominio se encarga de sumar el uso y desactivarlo si llega al límite
        coupon.IncrementUse();

        await _repository.UpdateCouponAsync(coupon, cancellationToken);
    }

    public async Task AddProductReviewAsync(Guid productId, string userId, string customerName, int rating, string comment, CancellationToken cancellationToken)
    {
        var review = ProductReview.Create(productId, userId, customerName, rating, comment);
        await _repository.AddReviewAsync(review, cancellationToken);
    }

    public async Task<IEnumerable<ProductReview>> GetApprovedReviewsForProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _repository.GetApprovedReviewsByProductAsync(productId, cancellationToken);
    }

    public async Task<IEnumerable<ProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetPendingReviewsAsync(cancellationToken);
    }

    public async Task ApproveReviewAsync(Guid reviewId, CancellationToken cancellationToken)
    {
        var review = await _repository.GetReviewByIdAsync(reviewId, cancellationToken);
        if (review == null) throw new Exception("Reseña no encontrada.");

        review.Approve();

        await _repository.UpdateReviewAsync(review, cancellationToken);
    }
}