namespace YuyoDev.Application.Services;

using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Entities.CMS;

public class StoreConfigurationService : IStoreConfigurationService
{
    private readonly IStoreConfigurationRepository _repository;

    public StoreConfigurationService(IStoreConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<StoreConfiguration> GetCurrentConfigurationAsync(CancellationToken cancellationToken)
    {
        var config = await _repository.GetConfigurationAsync(cancellationToken);

        // Si es la primera vez que se consulta y la base de datos está vacía,
        // creamos una configuración por defecto para que el Frontend no explote.
        if (config == null)
        {
            config = StoreConfiguration.Create("SLAccesorios");
            await _repository.AddConfigurationAsync(config, cancellationToken);
        }

        return config;
    }

    public async Task UpdateBrandingAsync(string logoUrl, string primaryColor, string secondaryColor, CancellationToken cancellationToken)
    {
        var config = await GetCurrentConfigurationAsync(cancellationToken);

        // Usamos el método de la entidad (Dominio Rico)
        config.UpdateBranding(logoUrl, primaryColor, secondaryColor);

        await _repository.UpdateConfigurationAsync(config, cancellationToken);
    }

    public async Task UpdateContactInfoAsync(string whatsappNumber, string instagramUrl, CancellationToken cancellationToken)
    {
        var config = await GetCurrentConfigurationAsync(cancellationToken);

        config.UpdateContactInfo(whatsappNumber, instagramUrl);

        await _repository.UpdateConfigurationAsync(config, cancellationToken);
    }
}