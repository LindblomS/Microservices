namespace Management.WebApp.Services;

public class TypeService : ITypeService
{
    public Task CreateAsync(string type)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<string>> GetAsync()
    {
        return await Task.FromResult(new[] { "asd", "asdf" });
    }
}
