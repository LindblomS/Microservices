namespace Shopping.WebApp.Services
{
    using System.Net.Http;
    using System.Text.Json;
    using Catalog.Contracts.Queries;
    using Microsoft.Extensions.Options;
    using Shopping.WebApp.Options;

    public class CatalogService
    {
        readonly IHttpClientFactory factory;
        readonly string uri;

        public CatalogService(IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            uri = options.Value.BaseAddress + options.Value.CatalogAddress;
        }

        public async Task<IReadOnlyCollection<Item>> GetAsync()
        {
            using var client = factory.CreateClient();
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
                return new List<Item>();

            return JsonSerializer.Deserialize<List<Item>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }
    }
}
