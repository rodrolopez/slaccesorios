using Microsoft.Extensions.Logging;
using YuyoDev.Application.Interfaces;

namespace YuyoDev.Infrastructure.Services;

public class LocalEmailService : IEmailService
{
    private readonly ILogger<LocalEmailService> _logger;

    public LocalEmailService(ILogger<LocalEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        // En lugar de enviar un mail real, lo pintamos en la consola
        _logger.LogWarning("=============================================");
        _logger.LogWarning("SIMULACIÓN DE ENVÍO DE CORREO (Yuyo Dev)");
        _logger.LogWarning($"Para: {to}");
        _logger.LogWarning($"Asunto: {subject}");
        _logger.LogWarning($"Cuerpo: {body}");
        _logger.LogWarning("=============================================");

        return Task.CompletedTask;
    }
}