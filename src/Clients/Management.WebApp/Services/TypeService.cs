namespace Management.WebApp.Services;

using Catalog.Contracts.Commands;
using Management.WebApp.Options;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

public class TypeService : ITypeService
{
    readonly IHttpClientFactory factory;
    readonly string uri;

    public TypeService(IHttpClientFactory factory, IOptions<ApiOptions> options)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        var apiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        uri = apiOptions.BaseAddress + apiOptions.TypeAddress;
    }

    public async Task CreateAsync(string type)
    {
        using var client = factory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(new CreateTypeCommand(type)), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<string>> GetAsync()
    {
        using var client = factory.CreateClient();
        var response = await client.GetAsync(uri);
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content))
            return new List<string>();

        return JsonSerializer.Deserialize<IEnumerable<string>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
