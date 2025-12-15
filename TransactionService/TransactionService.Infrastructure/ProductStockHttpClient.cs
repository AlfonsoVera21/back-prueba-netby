using System.Net.Http.Json;
using TransactionService.Domain.Ports;

namespace TransactionService.Infrastructure;

public class ProductStockHttpClient : IProductStockPort
{
    private readonly HttpClient _httpClient;

    public ProductStockHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private sealed record ProductDto(Guid Id, int Stock);

    public async Task<int> GetCurrentStockAsync(Guid productId)
    {
        var product = await _httpClient.GetFromJsonAsync<ProductDto>($"/api/products/{productId}");
        return product?.Stock ?? 0;
    }

    public async Task<bool> IncreaseStockAsync(Guid productId, int quantity)
    {
        var response = await _httpClient.PostAsync($"/api/products/{productId}/increase-stock?quantity={quantity}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DecreaseStockAsync(Guid productId, int quantity)
    {
        var response = await _httpClient.PostAsync($"/api/products/{productId}/decrease-stock?quantity={quantity}", null);
        return response.IsSuccessStatusCode;
    }
}
