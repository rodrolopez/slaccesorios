namespace YuyoDev.Application.Interfaces;

public interface IOrderProcessingJob
{
    // Este método será el que Hangfire ejecute en segundo plano
    Task ProcessApprovedPaymentAsync(Guid orderId, CancellationToken cancellationToken);
}