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

    [HttpPost("webhook/payment-approved/{orderId:guid}")]
    public IActionResult PaymentApprovedWebhook(Guid orderId)
    {
        _backgroundJobs.Enqueue<IOrderProcessingJob>(job => job.ProcessApprovedPaymentAsync(orderId, CancellationToken.None));
        return Ok(new { Message = "Webhook recibido. Procesando pago en segundo plano." });
    }

    [HttpGet]
    public IActionResult GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        // Usamos objetos anónimos para el Mock. El JSON resultante es idéntico
        // y el Frontend (Blazor) lo va a entender a la perfección.
        var mockOrders = new List<object>
        {
            new { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-1), UserId = "Cliente Local", TotalAmount = 45000.50m, Status = "Paid" },
            new { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddHours(-5), UserId = "Envío Moto", TotalAmount = 12500m, Status = "Pending" },
            new { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow.AddDays(-3), UserId = "Andreani", TotalAmount = 89000m, Status = "Shipped" }
        };

        var result = new
        {
            IsSuccess = true,
            Value = new
            {
                Items = mockOrders,
                TotalCount = mockOrders.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            }
        };

        return Ok(result); // Se envía como JSON y Blazor hace el trabajo sucio
    }
}