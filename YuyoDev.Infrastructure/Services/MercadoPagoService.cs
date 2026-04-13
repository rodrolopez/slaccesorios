namespace YuyoDev.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities;

public class MercadoPagoService : IMercadoPagoService
{
    private readonly IConfiguration _configuration;

    public MercadoPagoService(IConfiguration configuration)
    {
        _configuration = configuration;

        // Le pasamos el token al SDK leyendo de appsettings.json.
        // Si no lo encuentra, ponemos uno por defecto para que no explote ahora mismo.
        MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"]
                                        ?? "TEST-tu-access-token-de-prueba-aca";
    }

    public async Task<string> GeneratePaymentLinkAsync(Order order, CancellationToken cancellationToken)
    {
        // 1. Armamos el pedido a Mercado Pago
        var request = new PreferenceRequest
        {
            Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = $"Compra en SLAccesorios - Orden #{order.Id.ToString().Substring(0, 8)}",
                    Quantity = 1,
                    CurrencyId = "ARS",
                    UnitPrice = order.TotalAmount,
                }
            },
            // El ExternalReference es VITAL: es el ID que nos va a devolver Mercado Pago
            // en el webhook para que sepamos a qué orden corresponde el pago.
            ExternalReference = order.Id.ToString(),

            // A dónde mandamos al cliente después de pagar
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = "http://localhost:4200/checkout/success", // A futuro será tu dominio real
                Failure = "http://localhost:4200/checkout/failure",
                Pending = "http://localhost:4200/checkout/pending",
            },
            AutoReturn = "approved",

            // NotificationUrl = "https://tu-ngrok-url.app/api/checkout/webhook" // Para recibir el aviso por atrás
        };

        // 2. Enviamos el pedido a Mercado Pago
        var client = new PreferenceClient();
        Preference preference = await client.CreateAsync(request, requestOptions: null, cancellationToken: cancellationToken);

        // 3. Devolvemos la URL al cliente (usamos SandboxInitPoint porque estamos probando)
        // Cuando pases a producción, cambias esto por preference.InitPoint
        return preference.SandboxInitPoint;
    }
}