namespace YuyoDev.Domain.Entities.CMS;

public class StoreConfiguration
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string StoreName { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }

    // Branding
    public string PrimaryColorHex { get; private set; } = "#000000";
    public string SecondaryColorHex { get; private set; } = "#FFFFFF";

    // Contacto
    public string? WhatsAppNumber { get; private set; }
    public string? InstagramUrl { get; private set; }

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    protected StoreConfiguration() { }

    public void UpdateBranding(string logoUrl, string primary, string secondary)
    {
        LogoUrl = logoUrl;
        PrimaryColorHex = primary;
        SecondaryColorHex = secondary;
        UpdatedAt = DateTime.UtcNow;
    }
    public static StoreConfiguration Create(string storeName)
    {
        return new StoreConfiguration { StoreName = storeName };
    }

    public void UpdateContactInfo(string? whatsapp, string? instagram)
    {
        WhatsAppNumber = whatsapp;
        InstagramUrl = instagram;
        UpdatedAt = DateTime.UtcNow;
    }
}