namespace Shopping.WebApp.Services
{
    using System.Net.Http;
    using System.Text.Json;
    using Catalog.Contracts.Queries;
    using Microsoft.Extensions.Options;
    using Shopping.WebApp.Options;

    public class CatalogService : ICatalogService
    {
        readonly IHttpClientFactory factory;
        readonly JsonSerializerOptions jsonSerializerOptions;
        readonly string uri;

        public CatalogService(IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            uri = options.Value.BaseAddress + options.Value.CatalogAddress;
            jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IReadOnlyCollection<Item>> GetAsync()
        {
            using var client = factory.CreateClient();
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
                return new List<Item>();

            return JsonSerializer.Deserialize<List<Item>>(content, jsonSerializerOptions);
        }

        public async Task<Item> GetAsync(Guid id)
        {
            using var client = factory.CreateClient();
            var response = await client.GetAsync(uri + "/" + id);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Item>(content, jsonSerializerOptions);
        }
    }
}
