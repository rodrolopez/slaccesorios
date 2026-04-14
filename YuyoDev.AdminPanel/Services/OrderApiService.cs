using System.Net.Http.Json;
using YuyoDev.AdminPanel.Models;

namespace YuyoDev.AdminPanel.Services;

public class OrderApiService
{
    private readonly HttpClient _httpClient;

    public OrderApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResultDto<OrderDto>> GetOrdersAsync(int pageNumber, int pageSize)
    {
        // Llamada a la API que crearemos luego en los Controllers
        var response = await _httpClient.GetFromJsonAsync<Result<PagedResultDto<OrderDto>>>($"api/orders?pageNumber={pageNumber}&pageSize={pageSize}");

        if (response != null && response.IsSuccess)
        {
            return response.Value;
        }

        return new PagedResultDto<OrderDto>();
    }
}