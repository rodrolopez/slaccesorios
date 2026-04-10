namespace YuyoDev.Application.Interfaces;

public interface ITenantService
{
    // Este método nos va a devolver el ID del cliente actual (ej: "Teatro" o "Cerveceria")
    string GetTenant();
}