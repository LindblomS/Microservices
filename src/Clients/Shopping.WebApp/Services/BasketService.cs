namespace Shopping.WebApp.Services
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using Basket.Contracts.Models;
    using Microsoft.Extensions.Options;
    using Shopping.WebApp.Options;

    public class BasketService
    {
        readonly IHttpClientFactory factory;
        readonly string uri;

        public BasketService(IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            uri = options.Value.BaseAddress + options.Value.BasketAddress;
        }

        public async Task RemoveAsync(string productId)
        {
            using var client = factory.CreateClient();
            var respone = await client.DeleteAsync(uri + "/" + productId);
            respone.EnsureSuccessStatusCode();
        }

        public async Task AddAsync(BasketItem item)
        {
            using var client = factory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(new List<BasketItem> { item }), Encoding.UTF8, "application/json");
            var respone = await client.PostAsync(uri, content);
            respone.EnsureSuccessStatusCode();
        }
    }
}
