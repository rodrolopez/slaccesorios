using Microsoft.AspNetCore.Http;
using YuyoDev.Application.Interfaces;

namespace YuyoDev.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetTenant()
    {
        // Buscamos si la petición HTTP trae un encabezado llamado "X-Tenant-Id"
        var tenant = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();

        // Si no trae nada (como en nuestros Webhooks de prueba), le asignamos uno por defecto
        return string.IsNullOrEmpty(tenant) ? "YuyoDev_Default" : tenant;
    }
}