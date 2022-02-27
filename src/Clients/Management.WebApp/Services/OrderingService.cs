namespace Management.WebApp.Services;
using Management.WebApp.Options;
using Microsoft.Extensions.Options;
using Ordering.Contracts.Requests;
using System.Text.Json;

public class OrderingService
{
    readonly IHttpClientFactory factory;
    readonly string uri;

    public OrderingService(IHttpClientFactory factory, IOptions<ApiOptions> options)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        var apiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        uri = apiOptions.BaseAddress + apiOptions.OrderingAddress;
    }

    public async Task<GetOrder.Order> GetAsync(string id)
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri + "/" + id);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<GetOrder.Order>(
            await response.Content.ReadAsStringAsync(), 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }

    public async Task<IEnumerable<GetOrders.Order>> GetAsync()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
            return new List<GetOrders.Order>();

        return JsonSerializer.Deserialize<IEnumerable<GetOrders.Order>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
