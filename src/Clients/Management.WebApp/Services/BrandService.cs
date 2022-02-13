namespace Management.WebApp.Services;

public class BrandService : IBrandService
{
    public Task CreateAsync(string brand)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<string>> GetAsync()
    {
        return await Task.FromResult(new[] { "asdf", "asdf"});
    }
}
