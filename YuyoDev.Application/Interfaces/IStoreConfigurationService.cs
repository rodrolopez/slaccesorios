namespace YuyoDev.Application.Interfaces;

using YuyoDev.Domain.Entities.CMS;

public interface IStoreConfigurationService
{
    Task<StoreConfiguration> GetCurrentConfigurationAsync(CancellationToken cancellationToken);
    Task UpdateBrandingAsync(string logoUrl, string primaryColor, string secondaryColor, CancellationToken cancellationToken);
    Task UpdateContactInfoAsync(string whatsappNumber, string instagramUrl, CancellationToken cancellationToken);
}