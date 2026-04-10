using Microsoft.AspNetCore.Mvc;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;
using YuyoDev.Application.Interfaces;
using Hangfire; // <-- 1. Agregamos el using de Hangfire

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IBackgroundJobClient _backgroundJobClient; // <-- 2. Inyectamos Hangfire

    public WebhookController(ApplicationDbContext context, IBackgroundJobClient backgroundJobClient)
    {
        _context = context;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpPost("{source}")]
    public async Task<IActionResult> Receive(string source, [FromBody] object payload)
    {
        // 1. Logueamos en la base de datos de inmediato
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

        // 2. Preparamos los datos del correo
        string clienteEmail = "cliente@donatadeldesierto.com";
        string asunto = $"¡Pago recibido desde {source}!";
        string mensaje = $"Hola. Tu pago se procesó correctamente. Detalles: {payload}";

        // 3. LA MAGIA: Le pasamos la tarea a Hangfire y nos olvidamos.
        // Hangfire instanciará IEmailService en un hilo de fondo y ejecutará el método.
        _backgroundJobClient.Enqueue<IEmailService>(mailService =>
            mailService.SendEmailAsync(clienteEmail, asunto, mensaje));

        // 4. Devolvemos el 200 OK a Mercado Pago en milisegundos
        return Ok(new { message = $"Webhook de {source} procesado. Tarea de correo encolada en Hangfire." });
    }
}