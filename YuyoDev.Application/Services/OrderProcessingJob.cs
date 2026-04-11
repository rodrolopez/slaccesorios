namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;

public class OrderProcessingJob : IOrderProcessingJob
{
    private readonly IOrderRepository _orderRepository;

    public OrderProcessingJob(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ProcessApprovedPaymentAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
        if (order == null) return;

        // 1. Cambiamos el estado usando el método del Dominio Rico
        order.MarkAsPaid();
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);

        // 2. Acá a futuro se agregaría la lógica de descontar stock de las variantes
        // 3. Acá a futuro se llamaría a IEmailService para mandar la factura
    }
}