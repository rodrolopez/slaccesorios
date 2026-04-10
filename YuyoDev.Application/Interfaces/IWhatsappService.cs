namespace YuyoDev.Application.Interfaces;

public interface IWhatsAppService
{
    // Solo le pasamos el número y el mensaje, de la complejidad se encarga la infraestructura
    Task<bool> SendMessageAsync(string phoneNumber, string message);
}