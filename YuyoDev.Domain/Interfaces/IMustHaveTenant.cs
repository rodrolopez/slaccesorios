namespace YuyoDev.Domain.Interfaces;

public interface IMustHaveTenant
{
    // Esta propiedad será la "etiqueta" que separa a cada cliente
    string TenantId { get; set; }
}