namespace Ordering.Infrastructure.Services;

using Catalog.Contracts.Queries;
using Microsoft.Extensions.Options;
using Ordering.Application.Services;
using Ordering.Infrastructure.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

public class CatalogService : ICatalogService
{
    readonly IHttpClientFactory factory;
    readonly CatalogApiOptions options;
    readonly JsonSerializerOptions jsonOptions;

    public CatalogService(IHttpClientFactory factory, IOptions<CatalogApiOptions> options)
    {
        this.factory = factory;
        jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        this.options = options.Value;
    }

    public async Task<IReadOnlyCollection<Item>> GetAsync(IEnumerable<Guid> productIds)
    {
        using var client = factory.CreateClient();
        var items = new List<Item>();

        foreach (var productId in productIds)
            items.Add(await GetItem(productId, client));

        return items;
    }

    async Task<Item> GetItem(Guid productId, HttpClient client)
    {
        var response = await client.GetAsync(options.GetItemUri(productId));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Item>(content, jsonOptions);
    }
}
