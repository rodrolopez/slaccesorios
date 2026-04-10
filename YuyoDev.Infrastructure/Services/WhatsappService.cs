using Microsoft.Extensions.Logging;
using YuyoDev.Application.Interfaces;
using System.Text.Json;
using System.Text;

namespace YuyoDev.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WhatsAppService> _logger;

    // Inyectamos el HttpClient (provisto por la fábrica de .NET) y el Logger para Serilog
    public WhatsAppService(HttpClient httpClient, ILogger<WhatsAppService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> SendMessageAsync(string phoneNumber, string message)
    {
        try
        {
            _logger.LogInformation("Preparando envío de WhatsApp para el número {PhoneNumber}", phoneNumber);

            // 1. Acá armaríamos el JSON exacto que pide Meta (WhatsApp API)
            var payload = new
            {
                messaging_product = "whatsapp",
                to = phoneNumber,
                type = "text",
                text = new { body = message }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // 2. SIMULACIÓN DE LLAMADA EXTERNA
            // En la vida real haríamos: await _httpClient.PostAsync("https://graph.facebook.com/v17.0/TU_NUMERO/messages", content);

            // Simulamos que la API de Meta tarda 1 segundo en responder
            await Task.Delay(1000);

            _logger.LogInformation("=============================================");
            _logger.LogInformation(" WHATSAPP SIMULADO ENVIADO CON ÉXITO");
            _logger.LogInformation(" Destino: {PhoneNumber}", phoneNumber);
            _logger.LogInformation(" Mensaje: {Message}", message);
            _logger.LogInformation("=============================================");

            return true;
        }
        catch (Exception ex)
        {
            // Si la API de Meta se cae, Serilog guarda el error exacto
            _logger.LogError(ex, "Error crítico al intentar enviar WhatsApp al número {PhoneNumber}", phoneNumber);
            return false;
        }
    }
}