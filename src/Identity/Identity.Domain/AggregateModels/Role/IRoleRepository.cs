namespace Services.Identity.Domain.AggregateModels.Role
{
    using Services.Identity.Domain.Domain.SeedWork;
    using System.Threading.Tasks;

    public interface IRoleRepository : IRepository<Role>
    {
        Task CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task GetAsync(string id);
    }
}
