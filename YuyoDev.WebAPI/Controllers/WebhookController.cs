using YuyoDev.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;
using YuyoDev.Application.Interfaces;
using Hangfire;

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public WebhookController(ApplicationDbContext context, IBackgroundJobClient backgroundJobClient)
    {
        _context = context;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpPost("{source}")]
    public async Task<IActionResult> Receive(string source, [FromBody] object payload)
    {
        // 1. Guardamos en la base de datos (con el TenantId automático)
        var log = new AuditLog
        {
            Action = $"Webhook Received: {source}",
            EntityName = "Webhook",
            UserId = "System",
            Timestamp = DateTime.UtcNow,
            Details = payload.ToString() ?? "Empty Payload"
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();

        // 2. Preparamos los datos para los distintos canales
        string clienteEmail = "cliente@donatadeldesierto.com";
        string asunto = $"¡Pago recibido desde {source}!";
        string mensajeEmail = $"Tu pago se procesó. Detalles: {payload}";

        string celularAdmin = "+5492641234567";
        string mensajeWsp = $"¡Nueva venta por {source}! Ya podés preparar el pedido. Info: {payload}";

        // 3. LA MAGIA: Encolamos ambas tareas en Hangfire y nos olvidamos
        _backgroundJobClient.Enqueue<IEmailService>(mail =>
            mail.SendEmailAsync(clienteEmail, asunto, mensajeEmail));

        _backgroundJobClient.Enqueue<IWhatsAppService>(wsp =>
            wsp.SendMessageAsync(celularAdmin, mensajeWsp));

        // 4. Devolvemos la respuesta estandarizada
        var respuesta = Result<object>.Ok(
            data: new { Source = source },
            message: $"Webhook de {source} procesado. Tareas encoladas en Hangfire."
        );

        return Ok(respuesta);
    }
}