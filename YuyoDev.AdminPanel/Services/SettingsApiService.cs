using System.Net.Http.Json;
using YuyoDev.AdminPanel.Models;

namespace YuyoDev.AdminPanel.Services;

public class SettingsApiService
{
    private readonly HttpClient _httpClient;

    public SettingsApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UpdateContactAsync(string whatsapp, string instagram)
    {
        var payload = new { WhatsApp = whatsapp, Instagram = instagram };
        var response = await _httpClient.PostAsJsonAsync("api/settings/contact", payload);
        return response.IsSuccessStatusCode;
    }
}