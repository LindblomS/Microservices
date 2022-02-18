namespace Management.WebApp.Services;

using Catalog.Contracts.Commands;
using Catalog.Contracts.Queries;
using Management.WebApp.Models;
using Management.WebApp.Options;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

public class CatalogService : ICatalogService
{
    readonly IHttpClientFactory factory;
    readonly string uri;

    public CatalogService(IHttpClientFactory factory, IOptions<ApiOptions> options)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        var apiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        uri = apiOptions.BaseAddress + apiOptions.CatalogAddress;
    }

    public async Task CreateAsync(CreateCatalogItem item)
    {
        var request = new CreateItemCommand(
            item.Name,
            item.Description,
            item.Price,
            item.Type,
            item.Brand,
            item.AvailableStock);

        using var client = factory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        using var client = factory.CreateClient();
        var response = await client.DeleteAsync(uri + "/" + id);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<Item>> GetAsync()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content))
            return new List<Item>();

        return JsonSerializer.Deserialize<List<Item>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }

    public async Task<Item> GetAsync(string id)
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri + "/" + id);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Item>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task UpdateAsync(UpdateCatalogItem item)
    {
        var request = new UpdateItemCommand(
            item.Name,
            item.Description,
            item.Price,
            item.Type,
            item.Brand,
            item.AvailableStock);

        using var client = factory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await client.PutAsync(uri + "/" + item.Id, content);
        response.EnsureSuccessStatusCode();
    }
}
