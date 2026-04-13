namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;
using YuyoDev.Domain.Entities.Sales;

public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMercadoPagoService _mercadoPagoService;

    public CheckoutService(
        ICartService cartService,
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IMercadoPagoService mercadoPagoService)
    {
        _cartService = cartService;
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _mercadoPagoService = mercadoPagoService;
    }

    public async Task<string> ProcessCheckoutAsync(string userId, string sessionId, Guid shippingMethodId, CancellationToken cancellationToken)
    {
        // 1. Traemos el carrito
        var cart = await _cartService.GetCartAsync(userId, sessionId, cancellationToken);
        if (!cart.Items.Any()) throw new Exception("El carrito está vacío.");

        // 2. Creamos la Orden usando el método estático de tu dominio
        var order = Order.Create(userId ?? "Invitado");
        foreach (var item in cart.Items)
        {
            order.AddItem(item.ProductVariantId, item.Quantity, item.UnitPrice);
        }
        await _orderRepository.AddOrderAsync(order, cancellationToken);

        // 3. Generamos la transacción inicial en estado "Pendiente"
        var transaction = PaymentTransaction.Create(order.Id, order.TotalAmount, "MercadoPago", "Pending", null);
        await _paymentRepository.AddTransactionAsync(transaction, cancellationToken);

        // 4. Llamamos a Mercado Pago para que nos dé el link
        var paymentLink = await _mercadoPagoService.GeneratePaymentLinkAsync(order, cancellationToken);

        // 5. Opcional: Vaciar el carrito acá (o hacerlo cuando el pago se apruebe)

        return paymentLink;
    }

    public async Task HandlePaymentWebhookAsync(string paymentId, string status, CancellationToken cancellationToken)
    {
        var transaction = await _paymentRepository.GetTransactionByExternalReferenceAsync(paymentId, cancellationToken);
        if (transaction == null) return; // O loguear error

        transaction.UpdateStatus(status, DateTime.UtcNow);
        await _paymentRepository.UpdateTransactionAsync(transaction, cancellationToken);

        if (status.ToLower() == "approved")
        {
            var order = await _orderRepository.GetOrderByIdAsync(transaction.OrderId, cancellationToken);
            if (order != null)
            {
                order.MarkAsPaid();
                await _orderRepository.UpdateOrderAsync(order, cancellationToken);

                // Acá generarías la Factura (Invoice)
                var invoice = Invoice.Create(order.Id, $"FC-{order.Id.ToString().Substring(0, 8)}", "Consumidor Final", order.UserId, order.TotalAmount, null);
                await _paymentRepository.AddInvoiceAsync(invoice, cancellationToken);
            }
        }
    }
}