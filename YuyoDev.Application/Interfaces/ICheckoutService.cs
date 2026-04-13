namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;

public interface ICheckoutService
{
    // Transforma el carrito en una Orden y genera el link de pago
    Task<string> ProcessCheckoutAsync(string userId, string sessionId, Guid shippingMethodId, CancellationToken cancellationToken);

    // El webhook que recibe la respuesta de Mercado Pago
    Task HandlePaymentWebhookAsync(string paymentId, string status, CancellationToken cancellationToken);
}