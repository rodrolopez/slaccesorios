using System.Net.Http.Json;
using YuyoDev.AdminPanel.Models;

namespace YuyoDev.AdminPanel.Services;

public class CatalogApiService
{
    private readonly HttpClient _httpClient;

    public CatalogApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResultDto<ProductDto>> GetProductsAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var url = $"api/products?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            url += $"&searchTerm={searchTerm}";
        }

        // Asumiendo que tu endpoint devuelve un Result<PagedResultDto<ProductDto>>
        var response = await _httpClient.GetFromJsonAsync<Result<PagedResultDto<ProductDto>>>(url);

        if (response != null && response.IsSuccess)
        {
            return response.Value;
        }

        return new PagedResultDto<ProductDto>(); // Retorna vacío si falla


    }
    public async Task<bool> CreateProductAsync(CreateProductDto product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/products", product);
        return response.IsSuccessStatusCode;
    }
}