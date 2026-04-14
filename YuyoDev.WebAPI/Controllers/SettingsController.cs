namespace YuyoDev.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using YuyoDev.Application.Interfaces;
using YuyoDev.Domain.Shared;

public class SettingsController : BaseApiController
{
    private readonly IStoreConfigurationService _configService;

    public SettingsController(IStoreConfigurationService configService)
    {
        _configService = configService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
    {
        var config = await _configService.GetCurrentConfigurationAsync(cancellationToken);
        return Ok(Result<object>.Success(new {
            StoreName = config.StoreName,
            WhatsAppNumber = config.WhatsAppNumber,
            InstagramUrl = config.InstagramUrl
            // Eliminamos PrimaryColor temporalmente para que compile
        }));
    }

    [HttpPost("contact")]
    public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDto request, CancellationToken cancellationToken)
    {
        await _configService.UpdateContactInfoAsync(request.WhatsApp, request.Instagram, cancellationToken);
        return Ok(Result<string>.Success("Contacto actualizado"));
    }
}

public class UpdateContactDto
{
    public string WhatsApp { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;
}