namespace Management.WebApp.Services;

using Management.WebApp.Options;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

public class BrandService : IBrandService
{
    readonly IHttpClientFactory factory;
    readonly string uri;

    public BrandService(IHttpClientFactory factory, IOptions<ApiOptions> options)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        var apiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        uri = apiOptions.BaseAddress + "/" + apiOptions.BrandAddress;
    }

    public async Task CreateAsync(string brand)
    {
        using var client = factory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(brand), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<string>> GetAsync()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content))
            return new List<string>();

        return JsonSerializer.Deserialize<IEnumerable<string>>(content);
    }
}
