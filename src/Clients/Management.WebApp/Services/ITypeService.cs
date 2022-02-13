namespace Management.WebApp.Services;

public interface ITypeService
{
    Task CreateAsync(string type);
    Task<IEnumerable<string>> GetAsync();
}
