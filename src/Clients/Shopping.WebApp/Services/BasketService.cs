namespace Shopping.WebApp.Services
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using Basket.Contracts.Models;
    using Microsoft.Extensions.Options;
    using Shopping.WebApp.Options;

    public class BasketService : IBasketService
    {
        readonly IHttpClientFactory factory;
        readonly ICatalogService catalogService;
        readonly JsonSerializerOptions jsonSerializerOptions;
        readonly string uri;

        public BasketService(IHttpClientFactory factory, IOptions<ApiOptions> options, ICatalogService catalogService)
        {
            this.factory = factory;
            this.catalogService = catalogService;
            uri = options.Value.BaseAddress + options.Value.BasketAddress;
            jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task DeleteAsync(Guid buyerId)
        {
            using var client = factory.CreateClient();
            var respone = await client.DeleteAsync(uri + "/" + buyerId);
            respone.EnsureSuccessStatusCode();
        }

        public async Task AddAsync(Guid buyerId, Guid productId)
        {
            using var client = factory.CreateClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new { productId = productId.ToString() }), 
                Encoding.UTF8, 
                "application/json");

            var respone = await client.PostAsync(uri + "/" + buyerId, content);
            respone.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyCollection<Models.BasketItem>> GetAsync(Guid buyerId)
        {
            using var client = factory.CreateClient();
            var response = await client.GetAsync(uri + "/" + buyerId);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var basketItems = JsonSerializer.Deserialize<List<BasketItem>>(content, jsonSerializerOptions);
            var items = new List<Models.BasketItem>();

            foreach (var basketItem in basketItems)
            {
                var catalogItem = await catalogService.GetAsync(basketItem.ProductId);
                items.Add(new(catalogItem.Name, catalogItem.Price, basketItem.Units));
            }

            return items;
        }
    }
}
