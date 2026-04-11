namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.Interfaces;
using YuyoDev.Application.DTOs.Orders;
using Hangfire;

public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IBackgroundJobClient _backgroundJobs;

    public OrdersController(IOrderService orderService, IBackgroundJobClient backgroundJobs)
    {
        _orderService = orderService;
        _backgroundJobs = backgroundJobs;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken cancellationToken)
    {
        var result = await _orderService.CreateOrderAsync(request, cancellationToken);
        return HandleResult(result);
    }

    // Este es el endpoint que llamaría MercadoPago o Stripe
    [HttpPost("webhook/payment-approved/{orderId:guid}")]
    public IActionResult PaymentApprovedWebhook(Guid orderId)
    {
        // Encolamos el trabajo en Hangfire y respondemos 200 OK al instante a la pasarela de pagos
        _backgroundJobs.Enqueue<IOrderProcessingJob>(job => job.ProcessApprovedPaymentAsync(orderId, CancellationToken.None));

        return Ok(new { Message = "Webhook recibido. Procesando pago en segundo plano." });
    }
}