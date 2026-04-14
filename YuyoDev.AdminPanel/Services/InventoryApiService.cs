using System.Net.Http.Json;
using YuyoDev.AdminPanel.Models;

namespace YuyoDev.AdminPanel.Services;

public class InventoryApiService
{
    private readonly HttpClient _httpClient;

    public InventoryApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> AdjustStockAsync(Guid productVariantId, int quantity, string notes)
    {
        var payload = new { ProductVariantId = productVariantId, Quantity = quantity, Notes = notes };

        var response = await _httpClient.PostAsJsonAsync("api/inventory/adjust", payload);
        return response.IsSuccessStatusCode;
    }
}