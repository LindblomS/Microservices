namespace Management.WebApp.Services;

public interface IBrandService
{
    Task CreateAsync(string brand);
    Task<IEnumerable<string>> GetAsync();
}
