using Microsoft.AspNetCore.Mvc;
using YuyoDev.Domain.Entities;
using YuyoDev.Infrastructure.Persistence;

namespace YuyoDev.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WebhookController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Endpoint genérico para recibir notificaciones
    [HttpPost("{source}")] // Ej: api/webhook/mercadopago o api/webhook/whatsapp
    public async Task<IActionResult> Receive(string source, [FromBody] object payload)
    {
        // 1. Logueamos la recepción en nuestra nueva tabla de auditoría
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

        // 2. Aquí procesaríamos la lógica según el 'source'
        // Por ahora, solo devolvemos un 200 OK (es vital responder rápido a los webhooks)
        return Ok(new { message = $"Webhook de {source} procesado y auditado correctamente." });
    }
}