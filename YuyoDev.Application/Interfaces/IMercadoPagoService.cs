namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities;

public interface IMercadoPagoService
{
    // Le pasamos la Orden y nos devuelve el link (URL) para que el cliente pague
    Task<string> GeneratePaymentLinkAsync(Order order, CancellationToken cancellationToken);
}