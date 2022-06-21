namespace Shopping.WebApp.Services
{
    using Catalog.Contracts.Queries;

    public interface ICatalogService
    {
        Task<IReadOnlyCollection<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
    }
}
